using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private enum Direction { Up, Left, Down, Right, Null };

    private GameObject ghost;
    private Collider myCollider;
    public GameObject respawnPoint;

    public StateManager stateManager;
    public ScoreManager scoreManager;
    public MusicController musicController;

    private Animator animator;

    //Used to calculate direction and move Ghost                                                                     
    private Tweener tweener;                                    
        Direction lastInput;                                        
        Direction currentInput;                                     


    //Used for audio and visual feedback                             
    private SpriteRenderer spriteRenderer;

    public int ghostBehaviourCode;
    public bool inSpawnArea = true;
    private bool isDead = false;

    Vector3[] patrolPoints = new Vector3[138];
    public Transform[] allPatrolPoints = new Transform[20];
    private int currentPatrolPoint = 0;
    private Vector3 prevPatrolPoint;

    private bool startingPatrol = false;

    private GameObject pacStudent;
    private bool foundPlayer = true;

    public float patrolSpeed = 12f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghost = this.gameObject;
        myCollider = GetComponent<Collider>();
        tweener = GetComponent<Tweener>();
        pacStudent = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(stateManager.isPlayerDead() == true) 
        {
            pacStudent = null;
            foundPlayer = false;
        }

        else if (stateManager.isPlayerDead() == false && foundPlayer == false)
        {
            pacStudent = GameObject.FindWithTag("Player");
            foundPlayer = true;
        }


        if (stateManager.getState() == StateManager.GameState.Normal ||
            stateManager.getState() == StateManager.GameState.Scared ||
            stateManager.getState() == StateManager.GameState.Recovering)

        {
            if(inSpawnArea == false && isDead == false) 
            {
                if (ghostBehaviourCode == 1 || stateManager.getState() == StateManager.GameState.Scared || stateManager.getState() == StateManager.GameState.Recovering)
                {
                    startingPatrol = false;
                    ghostMovement1();
                }
                else if (ghostBehaviourCode == 2)
                {
                    ghostMovement2();
                }
                else if (ghostBehaviourCode == 3)
                {
                    ghostMovement3();
                }
                else if (ghostBehaviourCode == 4)
                {
                    ghostMovement4();
                }
            }
            else if (isDead == true && inSpawnArea == false) 
            {
                returnToSpawnArea();
            }

            if (Vector3.Distance(transform.position, respawnPoint.transform.position) < 0.1f && isDead == true)
            {
                isDead = false;
                leaveSpawnArea(ghostBehaviourCode);
            }
        }
        determineGhostState();
    }

    private void ghostMovement1() 
    {
        if(nearPlayer() == false) 
        {
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
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }

            }
        }
        else 
        {
            lastInput = getDirectionAwayFromPlayer();

            //If the gameobject is not currently moving
            if (tweener.getActiveTween() == null)
            {
                moveGhost(lastInput);
                if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }
            }
        }
    }

    private void ghostMovement2()
    {
        lastInput = getRandomDirection();

        if (playerIsVisible(lastInput) == false)
        {
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
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }

            }
        }
        else
        {
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
            else if(checkDirection(currentInput) == "Teleporter") 
            {
                exitTeleporter();
            }

        }
    }

    private void ghostMovement4() 
    { 
        if(startingPatrol == false) 
        {
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
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    exitTeleporter();
                }

            }
        }
        else 
        {
            if(currentPatrolPoint < allPatrolPoints.Length - 1) 
            {
                float distance = Vector3.Distance(allPatrolPoints[currentPatrolPoint].position, allPatrolPoints[currentPatrolPoint+1].position);

                if (tweener.getActiveTween() == null)
                {
                    if (allPatrolPoints[currentPatrolPoint + 1].position.x > transform.position.x) { animator.SetInteger("direction", (int)Direction.Right); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.x < transform.position.x) { animator.SetInteger("direction", (int)Direction.Left); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.y > transform.position.y) { animator.SetInteger("direction", (int)Direction.Up); }
                    else if (allPatrolPoints[currentPatrolPoint + 1].position.y < transform.position.y) { animator.SetInteger("direction", (int)Direction.Down); }

                    tweener.AddTween(ghost.transform, transform.position, allPatrolPoints[currentPatrolPoint + 1].position, (10f/distance) * Time.deltaTime);
                }

                if (allPatrolPoints[currentPatrolPoint + 1].position.x == allPatrolPoints[currentPatrolPoint].position.x) 
                {
                    alignToGrid("x");
                }
                else if(allPatrolPoints[currentPatrolPoint + 1].position.y == allPatrolPoints[currentPatrolPoint].position.y)
                {
                    alignToGrid("y");
                }

               // transform.position = Vector3.MoveTowards(transform.position, allPatrolPoints[currentPatrolPoint + 1].position, (patrolSpeed * Time.deltaTime)/distance);
            }
            else 
            {
                if (allPatrolPoints[0].position.x == allPatrolPoints[currentPatrolPoint].position.x)
                {
                    alignToGrid("x");
                }
                else if (allPatrolPoints[0].position.y == allPatrolPoints[currentPatrolPoint].position.y)
                {
                    alignToGrid("y");
                }

                float distance = Vector3.Distance(transform.position, allPatrolPoints[0].position);
                

                if (tweener.getActiveTween() == null)
                {
                    if (allPatrolPoints[1].position.x > transform.position.x) { animator.SetInteger("direction", (int)Direction.Right); }
                    else if (allPatrolPoints[1].position.x < transform.position.x) { animator.SetInteger("direction", (int)Direction.Left); }
                    else if (allPatrolPoints[1].position.y > transform.position.y) { animator.SetInteger("direction", (int)Direction.Up); }
                    else if (allPatrolPoints[1].position.y < transform.position.y) { animator.SetInteger("direction", (int)Direction.Down); }

                    tweener.AddTween(ghost.transform, transform.position, allPatrolPoints[0].position, (10f / distance) * Time.deltaTime);
                }

                //transform.position = Vector3.MoveTowards(transform.position, allPatrolPoints[0].position, (patrolSpeed * Time.deltaTime) / distance);
            }
                
        }
    }

    //Sets lastInput to a direction that has been generated
    private Direction getRandomDirection()
    {
        int randDirection = Random.Range(0, 4);

        if (randDirection == 0 && (int)currentInput != 2)
        {
            //Debug.Log(randDirection);
            return Direction.Up;
        }

        if (randDirection == 1 && (int)currentInput != 3)
        {
            //Debug.Log(randDirection);
            return Direction.Left;
        }

        if (randDirection == 2 && (int)currentInput != 0)
        {
            //Debug.Log(randDirection);
            return Direction.Down;
        }

        if (randDirection == 3 && (int)currentInput != 1)
        {
            //Debug.Log(randDirection);
            return Direction.Right;
        }

        return lastInput;
    }

    private Direction getDirectionAwayFromPlayer() 
    {
        Direction playerDirection = Direction.Null;
        Direction myDirection = Direction.Null;

        if (pacStudent.transform.position.x > transform.position.x) { playerDirection = Direction.Right; }
        else if (pacStudent.transform.position.x < transform.position.x) { playerDirection = Direction.Left; }
        else if (pacStudent.transform.position.y > transform.position.y) { playerDirection = Direction.Up; }
        else if (pacStudent.transform.position.y < transform.position.y) { playerDirection = Direction.Down; }

        if (playerDirection == Direction.Up)
        {
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Down)
        {
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Left)
        {
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Right)
        {
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            else { myDirection = playerDirection; }
        }

        return myDirection;
    }

    private bool playerIsVisible(Direction direction) 
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, getDirectionOfRay(), Color.red, 0f);
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

    private Vector3 getDirectionOfRay() 
    {
        int direction = (int)animator.GetInteger("direction");

        switch (direction) 
        {
            case 0: return Vector3.up;
            case 2: return Vector3.down;
            case 1: return Vector3.left;
            case 3: return Vector3.right;
        }
        return Vector3.zero;
    }

    private Direction getDirectionTowardsPlayer()
    {
        Direction playerDirection = Direction.Null;

        if (pacStudent.transform.position.x > transform.position.x) { playerDirection = Direction.Right; }
        else if (pacStudent.transform.position.x < transform.position.x) { playerDirection = Direction.Left; }
        else if (pacStudent.transform.position.y > transform.position.y) { playerDirection = Direction.Up; }
        else if (pacStudent.transform.position.y < transform.position.y) { playerDirection = Direction.Down; }

       /* if (playerDirection == Direction.Up)
        {
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Down)
        {
            if (checkDirection(Direction.Left) == "Walkable")
            {
                myDirection = Direction.Left;
            }
            else if (checkDirection(Direction.Right) == "Walkable")
            {
                myDirection = Direction.Right;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Left)
        {
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            else { myDirection = playerDirection; }
        }

        else if (playerDirection == Direction.Right)
        {
            if (checkDirection(Direction.Up) == "Walkable")
            {
                myDirection = Direction.Up;
            }
            else if (checkDirection(Direction.Down) == "Walkable")
            {
                myDirection = Direction.Down;
            }
            else { myDirection = playerDirection; }
        }*/

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

    private void moveGhostAway() 
    {
        Direction playerDirection = Direction.Null;
        Direction myDirection = Direction.Null;

        if(pacStudent.transform.position.x > transform.position.x) { playerDirection = Direction.Right; }
        else if(pacStudent.transform.position.x < transform.position.x) { playerDirection = Direction.Left; }
        else if(pacStudent.transform.position.y > transform.position.y) { playerDirection = Direction.Up; }
        else if(pacStudent.transform.position.y < transform.position.y) { playerDirection = Direction.Down; }

        if(playerDirection == Direction.Up) { myDirection = Direction.Down; }
        else if (playerDirection == Direction.Down) { myDirection = Direction.Up; }
        else if (playerDirection == Direction.Left) { myDirection = Direction.Right; }
        else if (playerDirection == Direction.Right) { myDirection = Direction.Left; }

        moveGhost(myDirection);
    }

    //Used to check if the ghost can travel in the given direction
    private string checkDirection(Direction direction)
    {

        //Temporary Vector3 stores the next coordinate
        Vector3 nextPos;

        if (direction == Direction.Up)
        {
            //nextPos set to grid position north of player
            nextPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y + 1), 0f);
        }
        else if (direction == Direction.Left)
        {
            //nextPos set to grid position west of player
            nextPos = new Vector3(Mathf.Round(transform.position.x - 1), Mathf.Floor(transform.position.y), 0f);
        }
        else if (direction == Direction.Down)
        {
            //nextPos set to grid position south of player
            nextPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y - 1), 0f);
        }
        else if (direction == Direction.Right)
        {
            //nextPos set to grid position east of player
            nextPos = new Vector3(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y), 0f);
        }
        else
        {
            nextPos = Vector3.zero;
        }

        if (nextPos.x <= 0) { nextPos.x = 0; } else if (nextPos.x >= LevelGenerator.getColumns() - 1) { nextPos.x = LevelGenerator.getColumns() - 1; }

        if (nextPos.y <= 0) { nextPos.x = 0; } else if (nextPos.y >= LevelGenerator.getRows() - 1) { nextPos.y = LevelGenerator.getRows() - 1; }


        if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 0 ||   //If the nextPos coordinates don't contain anything
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 5 ||   //If the nextPos coordinates contain a pellet
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 6)     //If the nextPos coordinates contain a power pellet
        {

            return "Walkable";
        }

        else if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 8)
        {
            return "Teleporter";
        }

        else if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 1)
        {
            return "OutsideCorner";
        }

        else if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 2)
        {
            return "OutsideWall";
        }

        else if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 4)
        {
            return "InsideWall";
        }

        //The ghost can't travel in the given direction
        return "NotWalkable";
    }

    //Used to check if the player is near the ghost or not
    private bool nearPlayer() 
    {
        if(foundPlayer == true) 
        {
            return Vector3.Distance(pacStudent.transform.position, transform.position) < 1f;
        }

        return false;
    }

    private void alignToGrid(string axis) 
    {
        switch (axis)
        {
            case "x": transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0f); break;
            case "y": transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0f); break;
        }
    }

    private bool chasingDistanceToPlayer()
    {
        if (foundPlayer == true)
        {
            return Vector3.Distance(pacStudent.transform.position, transform.position) < 4f;
        }

        return false;
    }

    private void leaveSpawnArea(int behaviourCode) 
    {
        myCollider.enabled = true;

        Vector3 exitTop = new Vector3(ghost.transform.position.x - 0.5f, respawnPoint.transform.position.y + 3, 0f);
        Vector3 exitBottom = new Vector3(ghost.transform.position.x + -0.5f, respawnPoint.transform.position.y - 3, 0f);

        bool inMiddle = false;

        //Debug.Log(ghostBehaviourCode + " is in the Spawn Area");
        //inSpawnArea = true;
        

        if(ghost.transform.position != respawnPoint.transform.position && inMiddle == false)
        {
            tweener.AddTween(ghost.transform, transform.position, respawnPoint.transform.position, 0.6f * Time.deltaTime);
        }
        else if (ghost.transform.position.x == 13.5f) 
        {
            inMiddle = true;
            if(behaviourCode == 1 || behaviourCode == 3) 
            {
                tweener.AddTween(ghost.transform, ghost.transform.position, exitTop, 0.1f * Time.deltaTime);
            }

            if (behaviourCode == 2 || behaviourCode == 4)
            {
                tweener.AddTween(ghost.transform, ghost.transform.position, exitBottom, 0.1f * Time.deltaTime);
            }
        }
    }

    private void returnToSpawnArea() 
    {
        transform.position = Vector3.MoveTowards(transform.position, respawnPoint.transform.position, 10f * Time.deltaTime);
    }

    private void exitTeleporter() 
    {
        if (ghost.transform.position.x == 1)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(6f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 1);
        }
        else if (ghost.transform.position.x == 26)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(21f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 3);
        }
    }

    private void determineGhostState() 
    {
        if(stateManager.getState() != StateManager.GameState.Awake && stateManager.getState() != StateManager.GameState.GameOver) 
        {
            if(isDead == false && inSpawnArea == false) 
            {
                if (stateManager.getState() == StateManager.GameState.Normal)
                {
                    animator.SetBool("normalPlay", true);

                    animator.SetBool("recovering", false);
                    animator.SetBool("scared", false);
                    animator.SetBool("dead", false);
                }
                else if (stateManager.getState() == StateManager.GameState.Scared)
                {
                    animator.SetBool("scared", true);

                    animator.SetBool("recovering", false);
                    animator.SetBool("normalPlay", false);
                    animator.SetBool("dead", false);
                }
                else if (stateManager.getState() == StateManager.GameState.Recovering)
                {
                    animator.SetBool("recovering", true);

                    animator.SetBool("scared", false);
                    animator.SetBool("normalPlay", false);
                    animator.SetBool("dead", false);
                }
            }
            else if( isDead == true && inSpawnArea == false)
            {
                animator.SetBool("dead", true);

                animator.SetBool("recovering", false);
                animator.SetBool("scared", false);
                animator.SetBool("normalPlay", false);
            }
            
        }
        else 
        {
            animator.SetBool("recovering", false);
            animator.SetBool("scared", false);
            animator.SetBool("normalPlay", false);
            animator.SetBool("dead", false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && stateManager.getState() != StateManager.GameState.Normal)
        {
            isDead = true;
            myCollider.enabled = false;
            scoreManager.AddScore(300);
            musicController.changeTrack(StateManager.GameState.OneDeadGhost);
        }
        else if(other.gameObject.tag == "Player" && stateManager.getState() == StateManager.GameState.Normal) 
        {
            stateManager.setPlayerDeath(true);
            foundPlayer = false;
        }
    }

    void OnCollisionExit(Collision other) 
    {
        if (other.gameObject.tag == "Player" && stateManager.getState() != StateManager.GameState.Normal)
        {
            isDead = true;
            returnToSpawnArea();
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            //Debug.Log("inside spanwer");
            inSpawnArea = true;
            isDead = false;
            leaveSpawnArea(ghostBehaviourCode);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            //Debug.Log("outside spanwer");
            inSpawnArea = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PatrolPoint" && ghostBehaviourCode == 4) 
        {
            currentPatrolPoint = System.Array.IndexOf(allPatrolPoints, other.gameObject.transform);
            startingPatrol = true;
            //Debug.Log("Array Index: " + currentPatrolPoint + ", startingPatrol: " + startingPatrol + ", length of array: " + allPatrolPoints.Length);
        }
    }
}


/*//If the ghost can travel in the given direction 
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
                else 
                {
                    Direction evacuate = getRandomDirection();
                    if(checkDirection(evacuate) == "Walkable") 
                    {
                        moveGhost(evacuate);
                        animator.SetInteger("direction", (int)evacuate);
                    }
                }*/
