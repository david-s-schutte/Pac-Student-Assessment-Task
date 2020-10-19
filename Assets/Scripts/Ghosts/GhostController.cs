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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghost = this.gameObject;
        tweener = GetComponent<Tweener>();
        currentPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

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
            else if(isDead == true) 
            {
                
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

        }
    }

    private void ghostMovement4() { }

    //Sets lastInput to a direction that has been generated
    private Direction getRandomDirection()
    {
        int randDirection = Random.Range(0, 4);

        //Update current position
        currentPos = ghost.transform.position;

        if (randDirection == 0 && (int)currentInput != 2 && nearSpawnPointEntrances() == false)
        {
            //Debug.Log(randDirection);
            return Direction.Up;
        }

        if (randDirection == 1 && (int)currentInput != 3 && nearLeftTeleporter() == false && nearSpawnPointEntrances() == false)
        {
            //Debug.Log(randDirection);
            return Direction.Left;
        }

        if (randDirection == 2 && (int)currentInput != 0 && nearSpawnPointEntrances() == false)
        {
            //Debug.Log(randDirection);
            return Direction.Down;
        }

        if (randDirection == 3 && (int)currentInput != 1 && nearRightTeleporter() == false && nearSpawnPointEntrances() == false)
        {
            //Debug.Log(randDirection);
            return Direction.Right;
        }

        return lastInput;
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

        //The player can't travel in the given direction
        return "NotWalkable";
    }


    private void leaveSpawnArea(int behaviourCode) 
    {
        //Debug.Log(ghostBehaviourCode + " is in the Spawn Area");
        inSpawnArea = true;
        if (isDead == true)
        {
            isDead = false;
            animator.SetBool("dead", false);
        }

        if(currentPos != respawnPoint.transform.position)
        {
            tweener.AddTween(ghost.transform, currentPos, respawnPoint.transform.position, 0.3f * Time.deltaTime);
        }
        else if (ghost.transform.position.x == 13.5f) 
        {
            Debug.Log("we're here");
        }

        /*Debug.Log("respawn pos: " + respawnPoint.transform.position);
        Debug.Log(gameObject.name + " pos: " + gameObject.transform.position);*/
    }

    private void returnToSpawnArea() 
    {
        if(tweener.getActiveTween() == null) 
        {
            tweener.AddTween(ghost.transform, currentPos, respawnPoint.transform.position, 0.3f * Time.deltaTime);
        } 
    }


    private bool nearLeftTeleporter() 
    {
        if((ghost.transform.position.y <= 15 && ghost.transform.position.y >= 13) && (ghost.transform.position.x <= 7 && ghost.transform.position.x >= 5)) 
        { 
             Debug.Log("Im near the left teleporter");
             return true;
        }
        return false;
    }

    private bool nearRightTeleporter()
    {
        if (ghost.transform.position.y == 14 && ghost.transform.position.x == 21)
        {
            Debug.Log("Im near the right teleporter");
            return true;
        }
        return false;
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
            
            animator.SetBool("scared", false);
            animator.SetBool("recovering", false);
            animator.SetBool("normalPlay", false);

            isDead = true;
            returnToSpawnArea();
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "GhostSpawner" && stateManager.getState() != StateManager.GameState.Awake) 
        {
            leaveSpawnArea(ghostBehaviourCode);
        }
    }
}

