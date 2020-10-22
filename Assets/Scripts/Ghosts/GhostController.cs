using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    //Enumerator used to store the direction the ghost is moving/facing
    private enum Direction { Up, Left, Down, Right, Null };


    //Used to calculate direction and move Ghost                                                                     
    private Tweener tweener;
    Direction lastInput;
    Direction currentInput;


    //Used for managing the ghost behaviour
    private GameObject ghost;
    private Collider myCollider;
    public GameObject respawnPoint;
    public StateManager stateManager;
    public int ghostBehaviourCode;   //Determines how the ghost will behave
    public bool inSpawnArea = true;
    private bool isDead = false;


    //Used for audio and visual feedback                             
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public MusicController musicController;
    public AudioClip oneDeadGhost;

  
    //Used for tracking the player
    public ScoreManager scoreManager;
    private GameObject pacStudent;
    private bool foundPlayer = true;


    //Variables used for Ghost 4
    private bool startingPatrol = false;
    public float patrolSpeed = 12f;
    public Transform[] allPatrolPoints = new Transform[20];
    private int currentPatrolPoint;


    // Start is called before the first frame update
    void Start()
    {
        //Initialises variables
        animator = GetComponent<Animator>();
        ghost = this.gameObject;
        myCollider = GetComponent<Collider>();
        tweener = GetComponent<Tweener>();
        pacStudent = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is dead
        if (stateManager.isPlayerDead() == true)
        {
            //Set reference to the player to null
            pacStudent = null;
            foundPlayer = false;
        }
        //If the player isn't dead
        else if (stateManager.isPlayerDead() == false && foundPlayer == false)
        {
            //Set reference to the player object to the player in the scene
            pacStudent = GameObject.FindWithTag("Player");
            foundPlayer = true;
        }

        //Determine which ghost state animation should be playing
        determineGhostState();

        //If the state in StateManager is normal, scared or recovering
        if (stateManager.getState() == StateManager.GameState.Normal ||
            stateManager.getState() == StateManager.GameState.Scared ||
            stateManager.getState() == StateManager.GameState.Recovering)

        {
            //If the ghost is neither dead or in the spawn area
            if (inSpawnArea == false && isDead == false)
            {
                //If the ghost is ghost 1 or if the state in StateManager is either scared or recovering
                if (ghostBehaviourCode == 1 || stateManager.getState() == StateManager.GameState.Scared || stateManager.getState() == StateManager.GameState.Recovering)
                {
                    //Force ghost 4 off patrol
                    if(ghostBehaviourCode == 4)
                    {
                        startingPatrol = false;
                    }
                    //Use ghost behaviour 1
                    ghostMovement1();
                }
                //If the ghost is ghost 2
                else if (ghostBehaviourCode == 2)
                {
                    //Use ghost behaviour2
                    ghostMovement2();
                }
                //If the ghost is ghost 3
                else if (ghostBehaviourCode == 3)
                {
                    //Use ghost behaviour 3
                    ghostMovement3();
                }
                //If the ghost is ghost 4
                else if (ghostBehaviourCode == 4)
                {
                    //Use ghost behaviour 4
                    ghostMovement4();
                }
            }
            //If the ghost is dead and not in the spawn area
            else if (isDead == true && inSpawnArea == false)
            {
                returnToSpawnArea();
            }
            //If the ghost is dead and in the spawn area
            if (Vector3.Distance(transform.position, respawnPoint.transform.position) < 0.1f && isDead == true)
            {
                //Bring ghost to life
                isDead = false;
                leaveSpawnArea(ghostBehaviourCode);
            }
        }
    }


    //Determines how the ghost will behave if ghostBehaviourCode is 1 or if the gameState in StateManager is scared or recovering
    private void ghostMovement1() 
    {
        //If the ghost isn't near the player
        if(nearPlayer() == false) 
        {
            //Get a random direction for the ghost to move in
            lastInput = getRandomDirection();

            //If the ghost is not currently moving
            if (tweener.getActiveTween() == null)
            {
                //If the ghost can travel in the given direction 
                if (checkDirection(lastInput) == "Walkable")
                {
                    currentInput = lastInput;
                    moveGhost(lastInput);                               //move ghost in this direction
                    animator.SetInteger("direction", (int)lastInput);   //set animator to match state of direction
                }
                //If the ghost can travel in the direction they are currently travelling
                else if (checkDirection(currentInput) == "Walkable")
                {
                    moveGhost(currentInput);                                //keep moving ghost in current direction
                    animator.SetInteger("direction", (int)currentInput);    //set animator to match state of direction
                }
                //If the ghost reaches a teleporter
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }
            }
        }
        //If the ghost is near the player
        else 
        {
            //Get a direction that allows the ghost to escape the player
            lastInput = getDirectionAwayFromPlayer();

            //If the ghost is not currently moving
            if (tweener.getActiveTween() == null)
            {
                moveGhost(lastInput);
                animator.SetInteger("direction", (int)lastInput);

                if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }
            }
        }
    }


    //Determines how the ghost will move if ghostBehaviourCode is 2
    private void ghostMovement2()
    {
        //Give the ghost a random direction to move in
        lastInput = getRandomDirection();

        //If the player isn't visible to the ghost, move randomly
        if (playerIsVisible(lastInput) == false)
        {
            //If the gahost is not currently moving
            if (tweener.getActiveTween() == null)
            {
                //If the ghost can travel in the given direction 
                if (checkDirection(lastInput) == "Walkable")
                {
                    currentInput = lastInput;
                    moveGhost(lastInput);                               //move ghost in this direction
                    animator.SetInteger("direction", (int)lastInput);   //set animator to match state of direction
                }
                //Else if the ghost can travel in the direction they are currently travelling
                else if (checkDirection(currentInput) == "Walkable")
                {
                    moveGhost(currentInput);                                //keep moving ghost in current direction
                    animator.SetInteger("direction", (int)currentInput);    //set animator to match state of direction
                }
                //If the ghost is about to enter a teleporter
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }
            }
        }
        //If the player is visible to the ghost
        else
        {
            //Give the ghost the same direction as the player
            lastInput = getDirectionTowardsPlayer();

            if (tweener.getActiveTween() == null)
            {
                //If the ghost can travel in the given direction 
                if (checkDirection(lastInput) == "Walkable")
                {
                    currentInput = lastInput;
                    moveGhost(lastInput);                               //move ghost in this direction
                    animator.SetInteger("direction", (int)lastInput);   //set animator to match state of direction
                }
                //Else if the ghost can travel in the direction they are currently travelling
                else if (checkDirection(currentInput) == "Walkable")
                {
                    moveGhost(currentInput);                                //keep moving ghost in current direction
                    animator.SetInteger("direction", (int)currentInput);    //set animator to match state of direction
                }
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }
            }
        }
    }


    //Determines how the ghost will move if ghostBehaviourCode is 3
    private void ghostMovement3()
    {
        //Gets a random direction for the ghost
        lastInput = getRandomDirection();

        //If the gameobject is not currently moving
        if (tweener.getActiveTween() == null)
        {
            //If the ghost can travel in the given direction 
            if (checkDirection(lastInput) == "Walkable")
            {
                currentInput = lastInput;
                moveGhost(lastInput);                               //move ghost in this direction
                animator.SetInteger("direction", (int)lastInput);   //set animator to match state of direction
            }
            //Else if the ghost can travel in the direction they are currently travelling
            else if (checkDirection(currentInput) == "Walkable")
            {
                moveGhost(currentInput);                                //keep moving ghost in current direction
                animator.SetInteger("direction", (int)currentInput);    //set animator to match state of direction
            }
            //If the ghost is about to enter a teleporter
            else if(checkDirection(currentInput) == "Teleporter") 
            {
                exitTeleporter();
            }
        }
    }


    //Determines how the ghost will move if ghostBehaviourCode is 4
    private void ghostMovement4() 
    { 
        //If the ghost has not yet found a patrol point, move it randomly until it finds one
        if(startingPatrol == false) 
        {
            //Move in random direction
            lastInput = getRandomDirection();

            //If the gameobject is not currently moving
            if (tweener.getActiveTween() == null)
            {
                //If the ghost can travel in the given direction 
                if (checkDirection(lastInput) == "Walkable")
                {
                    currentInput = lastInput;
                    moveGhost(lastInput);                               //move ghost in this direction
                    animator.SetInteger("direction", (int)lastInput);   //set animator to match state of direction
                }
                //Else if the ghost can travel in the direction they are currently travelling
                else if (checkDirection(currentInput) == "Walkable")
                {
                    moveGhost(currentInput);                                //keep moving ghost in current direction
                    animator.SetInteger("direction", (int)currentInput);    //set animator to match state of direction
                }
                //If the ghost is about to enter a teleporter
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }

            }
        }
        //If the ghost has found a patrol point
        else 
        {
            //If the patrolpoint reached by the ghost is not the last one in the array
            if(currentPatrolPoint < allPatrolPoints.Length - 1) 
            {
                //calculate the distance between the currentpatrol point reached and the next one
                float distance = Vector3.Distance(allPatrolPoints[currentPatrolPoint].position, allPatrolPoints[currentPatrolPoint+1].position);

                //If the ghost isn't already moving
                if (tweener.getActiveTween() == null)
                {
                    //Determines which way the ghost should have to face
                    if (allPatrolPoints[currentPatrolPoint + 1].position.x > transform.position.x) { animator.SetInteger("direction", (int)Direction.Right); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.x < transform.position.x) { animator.SetInteger("direction", (int)Direction.Left); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.y > transform.position.y) { animator.SetInteger("direction", (int)Direction.Up); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.y < transform.position.y) { animator.SetInteger("direction", (int)Direction.Down); }
                    
                    //Move the ghost to the next patrol point
                    tweener.AddTween(ghost.transform, transform.position, allPatrolPoints[currentPatrolPoint + 1].position, (distance/7f) * Time.deltaTime);
                }
                //if the next patrol point and the current patrol point are on the same X coordinate
                if (allPatrolPoints[currentPatrolPoint + 1].position.x == allPatrolPoints[currentPatrolPoint].position.x) 
                {
                    alignToGrid("x");
                }
                //if the next patrol point and the current patrol point are on the same Y coordinate
                else if (allPatrolPoints[currentPatrolPoint + 1].position.y == allPatrolPoints[currentPatrolPoint].position.y)
                {
                    alignToGrid("y");
                }
            }
            //If the patrol point reached by the ghost is the last one in the array
            else 
            {
                //calculate the distance between the currentpatrol point reached and the next one
                float distance = Vector3.Distance(transform.position, allPatrolPoints[0].position);

                //If the ghost isn't already moving
                if (tweener.getActiveTween() == null)
                {
                    //Determines which way the ghost should have to face
                    if (allPatrolPoints[1].position.x > transform.position.x) { animator.SetInteger("direction", (int)Direction.Right); }
                    else if (allPatrolPoints[1].position.x < transform.position.x) { animator.SetInteger("direction", (int)Direction.Left); }
                    else if (allPatrolPoints[1].position.y > transform.position.y) { animator.SetInteger("direction", (int)Direction.Up); }
                    else if (allPatrolPoints[1].position.y < transform.position.y) { animator.SetInteger("direction", (int)Direction.Down); }

                    //Move the ghost to the next patrol point
                    tweener.AddTween(ghost.transform, transform.position, allPatrolPoints[0].position, (distance / 7f) * Time.deltaTime);
                }
                //if the next patrol point and the current patrol point are on the same X coordinate
                if (allPatrolPoints[0].position.x == allPatrolPoints[currentPatrolPoint].position.x)
                {
                    alignToGrid("x");
                }
                //if the next patrol point and the current patrol point are on the same Y coordinate
                else if (allPatrolPoints[0].position.y == allPatrolPoints[currentPatrolPoint].position.y)
                {
                    alignToGrid("y");
                }
            }
                
        }
    }


    //Returns a direction that has been generated randomly between a certain range
    private Direction getRandomDirection()
    {
        int randDirection = Random.Range(0, 4);

        if (randDirection == 0 && (int)currentInput != 2)
        {
            return Direction.Up;
        }

        if (randDirection == 1 && (int)currentInput != 3)
        {
            return Direction.Left;
        }

        if (randDirection == 2 && (int)currentInput != 0)
        {
            return Direction.Down;
        }

        if (randDirection == 3 && (int)currentInput != 1)
        {
            return Direction.Right;
        }

        return lastInput;
    }


    //Returns a direction that allows them to move away from the player without moving backwards
    private Direction getDirectionAwayFromPlayer() 
    {
        //Temporary variables used to store the player's direction and the ghost's next direction
        Direction playerDirection = Direction.Null;
        Direction myDirection = Direction.Null;

        //Determines which direction the player is going
        if (pacStudent.transform.position.x > transform.position.x) { playerDirection = Direction.Right; }
        else if (pacStudent.transform.position.x < transform.position.x) { playerDirection = Direction.Left; }
        else if (pacStudent.transform.position.y > transform.position.y) { playerDirection = Direction.Up; }
        else if (pacStudent.transform.position.y < transform.position.y) { playerDirection = Direction.Down; }

        //If the player is going up
        if (playerDirection == Direction.Up)
        {
            //If can move left, move left
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            //If can move right, move right
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            //Otherwise keep moving in current direction
            else { myDirection = playerDirection; }
        }
        //If the player is going down
        else if (playerDirection == Direction.Down)
        {
            //If can move left, move left
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            //If can move right, move right
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            //Otherwise keep moving in current direction
            else { myDirection = playerDirection; }
        }
        //If the player is moving left
        else if (playerDirection == Direction.Left)
        {
            //If can move up, move up
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            //If can move down, move down
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            //Otherwise keep moving in current direction
            else { myDirection = playerDirection; }
        }
        //If the player is moving right
        else if (playerDirection == Direction.Right)
        {
            //If can move up, move up
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            //if can move down, move down
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            //Otherwise keep moving in current direction
            else { myDirection = playerDirection; }
        }

        return myDirection;
    }


    //Determines if the ghost can see the player (used by ghost 2)
    private bool playerIsVisible(Direction direction) 
    {
        //Temporary variable that stores what the raycast hit
        RaycastHit hit;
        //If the ghost can see the player from where they're facing
        if(Physics.Raycast(transform.position, getDirectionOfRay(), out hit, 100f)) 
        {
            if(hit.collider.gameObject.tag == "Player") 
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        return false;
    }


    //Determines the direction of the raycast used in playerIsVisible() based on the direction of the ghost (used by ghost 2)
    private Vector3 getDirectionOfRay() 
    {
        //temporary variable that stores the direction fo the ray - determined via currentInput
        int direction = (int)currentInput;

        //Determines which direction to send
        switch (direction) 
        {
            case 0: return Vector3.up;
            case 2: return Vector3.down;
            case 1: return Vector3.left;
            case 3: return Vector3.right;
        }
        return Vector3.zero;
    }


    //Returns the direction of the player
    private Direction getDirectionTowardsPlayer()
    {
        //Temporary variable that stores the player's direction
        Direction playerDirection = Direction.Null;

        //Determines the direction of the player
        if (pacStudent.transform.position.x > transform.position.x) { playerDirection = Direction.Right; }
        else if (pacStudent.transform.position.x < transform.position.x) { playerDirection = Direction.Left; }
        else if (pacStudent.transform.position.y > transform.position.y) { playerDirection = Direction.Up; }
        else if (pacStudent.transform.position.y < transform.position.y) { playerDirection = Direction.Down; }

        return playerDirection;
    }


    //Moves the ghost in the given direction
    private void moveGhost(Direction direction)
    {
        //Temporary Vector3 stores the next coordinate
        Vector3 nextPos;

        if (direction == Direction.Up)
        {
            //nextPos set to grid position north of player
            nextPos = new Vector3(transform.position.x, transform.position.y + 1, 0f);
        }
        else if (direction == Direction.Left)
        {
            //nextPos set to grid position west of player
            nextPos = new Vector3(transform.position.x - 1, transform.position.y, 0f);
        }
        else if (direction == Direction.Down)
        {
            //nextPos set to grid position south of player
            nextPos = new Vector3(transform.position.x, transform.position.y - 1, 0f);
        }
        else if (direction == Direction.Right)
        {
            //nextPos set to grid position east of player
            nextPos = new Vector3(transform.position.x + 1, transform.position.y, 0f);
        }
        else
        {
            //nextPos set to player's current position
            nextPos = transform.position;
        }

        //Tween player in direction of nextPos from currentPos
        tweener.AddTween(ghost.transform, transform.position, nextPos, 0.15f * Time.deltaTime);
    }


    //Used to check if the ghost can travel in the given direction
    private string checkDirection(Direction direction)
    {
        //Temporary Vector3 stores the next coordinate
        Vector3 nextPos;

        if (direction == Direction.Up)
        {
            //nextPos set to grid position north of ghost
            nextPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 1), 0f);
        }
        else if (direction == Direction.Left)
        {
            //nextPos set to grid position west of ghost
            nextPos = new Vector3(Mathf.Round(transform.position.x - 1), Mathf.Floor(transform.position.y), 0f);
        }
        else if (direction == Direction.Down)
        {
            //nextPos set to grid position south of ghost
            nextPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1), 0f);
        }
        else if (direction == Direction.Right)
        {
            //nextPos set to grid position east of ghost
            nextPos = new Vector3(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y), 0f);
        }
        else
        {
            nextPos = Vector3.zero;
        }

        //Prevents outofindexrange error
        if (nextPos.x <= 0) { nextPos.x = 0; } else if (nextPos.x >= LevelGenerator.getColumns() - 1) { nextPos.x = LevelGenerator.getColumns() - 1; }

        if (nextPos.y <= 0) { nextPos.x = 0; } else if (nextPos.y >= LevelGenerator.getRows() - 1) { nextPos.y = LevelGenerator.getRows() - 1; }


        if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 0 ||   //If the nextPos coordinates don't contain anything
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 5 ||   //If the nextPos coordinates contain a pellet
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 6)     //If the nextPos coordinates contain a power pellet
        {

            return "Walkable";
        }
        //If the nextPos contains a teleporter
        else if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 8)
        {
            return "Teleporter";
        }
        //The ghost can't travel in the given direction
        return "NotWalkable";
    }


    //Used to check if the player is near the ghost or not
    private bool nearPlayer() 
    {
        //If the player is alive
        if(foundPlayer == true) 
        {
            //Returns true if the player is one or less than one unit away from the ghost
            return Vector3.Distance(pacStudent.transform.position, transform.position) <= 1f;
        }
        return false;
    }


    //Aligns the ghost to grid (mainly used for Ghost 4 when patrolling)
    private void alignToGrid(string axis) 
    {
        //Determine which axis to align to
        switch (axis)
        {
            case "x": transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0f); break;
            case "y": transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0f); break;
        }
    }


    //Moves the ghost out of the spawn area - where they leave from depends on the ghost behaviour code
    private void leaveSpawnArea(int behaviourCode) 
    {
        //Re-enable the ghost collider
        myCollider.enabled = true;

        //Temporary variables that determine where each exist coordinate is
        Vector3 exitTop = new Vector3(ghost.transform.position.x - 0.5f, respawnPoint.transform.position.y + 3, 0f);
        Vector3 exitBottom = new Vector3(ghost.transform.position.x + -0.5f, respawnPoint.transform.position.y - 3, 0f);

        //Temporary variables that determines if the ghosts are in the middle of the spawn point
        bool inMiddle = false;

        //If the ghost isn't in the middle
        if(ghost.transform.position != respawnPoint.transform.position && inMiddle == false)
        {
            //Move to the middle
            tweener.AddTween(ghost.transform, transform.position, respawnPoint.transform.position, 0.6f * Time.deltaTime);
        }
        //If the ghost is in the middle
        else if (ghost.transform.position.x == 13.5f) 
        {
            inMiddle = true;

            //If the ghostBehaviourCode is either 1 or 3
            if(behaviourCode == 1 || behaviourCode == 3) 
            {
                //Exit through the top
                tweener.AddTween(ghost.transform, ghost.transform.position, exitTop, 0.1f * Time.deltaTime);
                animator.SetInteger("direction", (int)Direction.Up);
            }
            //If the ghostBehaviourCode is either 2 or 4
            if (behaviourCode == 2 || behaviourCode == 4)
            {
                //Exit through the bottom
                tweener.AddTween(ghost.transform, ghost.transform.position, exitBottom, 0.1f * Time.deltaTime);
                animator.SetInteger("direction", (int)Direction.Down);
            }
        }
    }


    //Moves the ghost to the spawn area - called when they die
    private void returnToSpawnArea() 
    {
        transform.position = Vector3.MoveTowards(transform.position, respawnPoint.transform.position, 10f * Time.deltaTime);
    }

    //Moves the ghost away from the teleporters
    private void exitTeleporter() 
    {
        //If they are at the leftmost teleporter
        if (ghost.transform.position.x == 1)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(6f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 1);
        }
        //If they are at the rightmost teleporter
        else if (ghost.transform.position.x == 26)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(21f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 3);
        }
    }


    //Determines which ghost state sprites to use
    private void determineGhostState() 
    {
        //If the game has already started and hasn't ended
        if(stateManager.getState() != StateManager.GameState.Awake && stateManager.getState() != StateManager.GameState.GameOver) 
        {
            //If the ghost isn't dead
            if(isDead == false) 
            {
                //If the gameState in StateManager is normal
                if (stateManager.getState() == StateManager.GameState.Normal)
                {
                    //use normal sprites
                    animator.SetBool("normalPlay", true);

                    animator.SetBool("recovering", false);
                    animator.SetBool("scared", false);
                    animator.SetBool("dead", false);
                }
                //If the gameState in StateManager is scared
                else if (stateManager.getState() == StateManager.GameState.Scared)
                {
                    //use scared sprites
                    animator.SetBool("scared", true);

                    animator.SetBool("recovering", false);
                    animator.SetBool("normalPlay", false);
                    animator.SetBool("dead", false);
                }
                //If the gameState in StateManager is recovering
                else if (stateManager.getState() == StateManager.GameState.Recovering)
                {
                    //use recovering sprites
                    animator.SetBool("recovering", true);

                    animator.SetBool("scared", false);
                    animator.SetBool("normalPlay", false);
                    animator.SetBool("dead", false);
                }
            }
            //If the ghost is dead
            else if( isDead == true)
            {
                //use dead sprite
                animator.SetBool("dead", true);

                animator.SetBool("recovering", false);
                animator.SetBool("scared", false);
                animator.SetBool("normalPlay", false);
            }        
        }
    }


    //Handles collisions upon first contact
    void OnCollisionEnter(Collision other)
    {
        //If a ghost touches the player while the game state in StateManager is not normal
        if (other.gameObject.tag == "Player" && stateManager.getState() != StateManager.GameState.Normal)
        {
            //The ghost is dead
            isDead = true;
            //Disable the ghost's collider
            myCollider.enabled = false;
            //Add 300 points to the score
            scoreManager.AddScore(300);
            //Change the music track to one dead ghost track if it isn't already playing
            if(musicController.getCurrentTrack() != oneDeadGhost) 
            {
                musicController.changeTrack(StateManager.GameState.OneDeadGhost);
            }
            //Return ghost to spawn area
            returnToSpawnArea();
        }
        //If a ghost touches the player while the game state in StateManager is normal
        else if (other.gameObject.tag == "Player" && stateManager.getState() == StateManager.GameState.Normal) 
        {
            //Kill the player
            stateManager.setPlayerDeath(true);
            //Set the player object within this script to null
            foundPlayer = false;
        }
    }


    //Handles collisions with trigger if the ghost continues to collide with it
    void OnTriggerStay(Collider other) 
    {
        //if colliding with GhostSpawner and the gameState in StateManager is not Awake
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            inSpawnArea = true;
            isDead = false;
            leaveSpawnArea(ghostBehaviourCode);
        }
    }


    //Handles collisions with trigger once collision ends
    void OnTriggerExit(Collider other) 
    {
        //if colliding with GhostSpawner and the gameState in StateManager is not Awake
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            inSpawnArea = false;
        }
    }


    //Handles collisions with trigger upon first contact
    void OnTriggerEnter(Collider other)
    {
        //If the object is a patrol point and the ghostBehaviour is 4
        if(other.gameObject.tag == "PatrolPoint" && ghostBehaviourCode == 4) 
        {
            //Determine which patrol point is being touched
            currentPatrolPoint = System.Array.IndexOf(allPatrolPoints, other.gameObject.transform);
            //Set the ghost to start patrolling
            startingPatrol = true;
        }
    }
}