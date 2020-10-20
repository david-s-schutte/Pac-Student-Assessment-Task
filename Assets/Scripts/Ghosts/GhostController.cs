using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private enum Direction { Up, Left, Down, Right, Null };

    private GameObject ghost;
    public GameObject respawnPoint;

    public StateManager stateManager;
    public ScoreManager scoreManager;
    public MusicController musicController;

    private Animator animator;

    //Used to calculate direction and move PacStudent
    private Vector3 currentPos;                                 //Stores current position of player                                    
    private Tweener tweener;                                    //Reference to tweener component attached to gameobject
        Direction lastInput;                                        //Stores the last input received
        Direction currentInput;                                     //Stores the current trajectory to travel in


    //Used for audio and visual feedback                             
    private SpriteRenderer spriteRenderer;

    public int ghostBehaviourCode;
    public bool inSpawnArea = true;
    private bool isDead = false;

    Vector3[] patrolPoints = new Vector3[20];
    public Transform[] allPatrolPoints = new Transform[20];
    private int currentPatrolPoint = 0;
    private Vector3 prevPatrolPoint;

    private GameObject pacStudent;
    private bool foundPlayer = true;

    void Awake() 
    {
        //allPatrolPoints[0] = new Vector3(1f, 27f, 0f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghost = this.gameObject;
        tweener = GetComponent<Tweener>();
        currentPos = gameObject.transform.position;
        pacStudent = GameObject.FindWithTag("Player");
        Debug.Log(pacStudent.name + " has been found");
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

        //Debug.Log(pacStudent);

        if (stateManager.getState() == StateManager.GameState.Normal ||
            stateManager.getState() == StateManager.GameState.Scared ||
            stateManager.getState() == StateManager.GameState.Recovering)

        {
            if(inSpawnArea == false && isDead == false) 
            {
                if (ghostBehaviourCode == 1 || stateManager.getState() == StateManager.GameState.Scared || stateManager.getState() == StateManager.GameState.Recovering)
                {
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

            if (stateManager.getState() == StateManager.GameState.Normal && !animator.GetBool("dead"))
            {
                animator.SetBool("normalPlay", true);
                animator.SetBool("scared", false);
                animator.SetBool("recovering", false);
            }

            else if (stateManager.getState() == StateManager.GameState.Scared && !animator.GetBool("dead"))
            {
                animator.SetBool("scared", true);
                animator.SetBool("recovering", false);
                animator.SetBool("normalPlay", false);
            }

            else if (stateManager.getState() == StateManager.GameState.Recovering && !animator.GetBool("dead"))
            {
                animator.SetBool("recovering", true);
                animator.SetBool("scared", false);
                animator.SetBool("normalPlay", false);
            }
        }
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

                moveGhost(lastInput);
            }
        }
    }

    private void ghostMovement2() { }

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
        
    }

    //Sets lastInput to a direction that has been generated
    private Direction getRandomDirection()
    {
        int randDirection = Random.Range(0, 4);

        //Update current position
        currentPos = ghost.transform.position;

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

    //Moves the ghost in the given direction
    private void moveGhost(Direction direction)
    {
        //Temporary Vector3 stores the next coordinate
        Vector3 nextPos;

        if (direction == Direction.Up)
        {
            //nextPos set to grid position north of player
            nextPos = new Vector3(currentPos.x, currentPos.y + 1, 0f);
        }
        else if (direction == Direction.Left)
        {
            //nextPos set to grid position west of player
            nextPos = new Vector3(currentPos.x - 1, currentPos.y, 0f);
        }
        else if (direction == Direction.Down)
        {
            //nextPos set to grid position south of player
            nextPos = new Vector3(currentPos.x, currentPos.y - 1, 0f);
        }
        else if (direction == Direction.Right)
        {
            //nextPos set to grid position east of player
            nextPos = new Vector3(currentPos.x + 1, currentPos.y, 0f);
        }
        else
        {
            //nextPos set to player's current position
            nextPos = currentPos;
        }

        //Tween player in direction of nextPos from currentPos
        tweener.AddTween(ghost.transform, currentPos, nextPos, 0.15f * Time.deltaTime);
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
            nextPos = new Vector3(currentPos.x, currentPos.y + 1, 0f);
        }
        else if (direction == Direction.Left)
        {
            //nextPos set to grid position west of player
            nextPos = new Vector3(currentPos.x - 1, currentPos.y, 0f);
        }
        else if (direction == Direction.Down)
        {
            //nextPos set to grid position south of player
            nextPos = new Vector3(currentPos.x, currentPos.y - 1, 0f);
        }
        else if (direction == Direction.Right)
        {
            //nextPos set to grid position east of player
            nextPos = new Vector3(currentPos.x + 1, currentPos.y, 0f);
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

    private void leaveSpawnArea(int behaviourCode) 
    {
        Vector3 exitTop = new Vector3(ghost.transform.position.x - 0.5f, respawnPoint.transform.position.y + 3, 0f);
        Vector3 exitBottom = new Vector3(ghost.transform.position.x + -0.5f, respawnPoint.transform.position.y - 3, 0f);

        bool inMiddle = false;

        //Debug.Log(ghostBehaviourCode + " is in the Spawn Area");
        inSpawnArea = true;
        

        if(ghost.transform.position != respawnPoint.transform.position && inMiddle == false)
        {
            tweener.AddTween(ghost.transform, currentPos, respawnPoint.transform.position, 0.3f * Time.deltaTime);
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
        if (tweener.getActiveTween() == null)
        {
            tweener.AddTween(ghost.transform, currentPos, respawnPoint.transform.position, 0.3f * Time.deltaTime);
        }
        //transform.position = Vector3.MoveTowards(transform.position, respawnPoint.transform.position, 1f * Time.deltaTime);
    }

    private void exitTeleporter() 
    {
        if (ghost.transform.position.x == 1)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(6f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 3);
        }
        else if (ghost.transform.position.x == 26)
        {
            tweener.AddTween(ghost.transform, ghost.transform.position, new Vector3(21f, 14f, 0f), 0.15f * Time.deltaTime);
            animator.SetInteger("direction", 1);
        }
    }

    private bool nearSpawnPointEntrances()
    {
        if (ghost.transform.position.y == 11 || ghost.transform.position.y == 17)
        {
            if (ghost.transform.position.x == 13 || ghost.transform.position.x == 14)
            {
                Debug.Log("Im at : x = " + ghost.transform.position.x + ", y = " + ghost.transform.position.y);
                return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && stateManager.getState() != StateManager.GameState.Normal)
        {
            scoreManager.AddScore(300);
            musicController.changeTrack(StateManager.GameState.OneDeadGhost);
            animator.SetBool("dead", true);
            isDead = true;
            animator.SetBool("scared", false);
            animator.SetBool("recovering", false);
            animator.SetBool("normalPlay", false);

            
            returnToSpawnArea();
        }
        else if(other.gameObject.tag == "Player") 
        {
            stateManager.setPlayerDeath(true);
            foundPlayer = false;
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake) 
        {
            leaveSpawnArea(ghostBehaviourCode);
            animator.SetBool("dead", false);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            inSpawnArea = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake)
        {
            if (isDead == true)
            {
                isDead = false;
                animator.SetBool("dead", false);
            }
        }

        if(other.gameObject.tag == "PatrolPoint" && ghostBehaviourCode == 4) 
        {
            prevPatrolPoint = other.gameObject.transform.position;
            patrolPoints[currentPatrolPoint] = prevPatrolPoint;

            if (currentPatrolPoint < 19) 
            {
                currentPatrolPoint++;
                Debug.Log(currentPatrolPoint);
            }
            else 
            {
                currentPatrolPoint = 0;
                Debug.Log(currentPatrolPoint);
            }

            Debug.Log(prevPatrolPoint);
        }
    }

    private void goToNextPoint() 
    {
        
    }
}

