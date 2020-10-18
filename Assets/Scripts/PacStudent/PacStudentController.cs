using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private bool started = false;

    //Used to calculate direction and move PacStudent
    private Vector3 currentPos;                                 //Stores current position of player
    private GameObject player;                                  //Reference to player object
    private Tweener tweener;                                    //Reference to tweener component attached to gameobject
    private enum Direction { Up, Left, Down, Right, Null };     //Enum used to determine which direction the player has inputted
        Direction lastInput;                                        //Stores the last input received
        Direction currentInput;                                     //Stores the current trajectory to travel in


    //Used for audio and visual feedback
    private ParticleSystem movementParticles;                   //Reference to particle system attached to gameobject
    private Animator animator;                                  //Reference to animator attached to gameobject
    public AudioClip[] audioClips = new AudioClip[2];           //Stores audioclips that pacstudent should play - check inspector
    public AudioSource eatPellet;                              //Reference to audio source system attached to gameobject
    public AudioSource walkingSound;
    public AudioSource collisionSound;
    public GameObject collisionParticles;                       //Reference to particle system for wall collisions
    public GameObject deathParticles;
    private bool colliding;
    private Vector3 collisionSpawnPos;
    private SpriteRenderer spriteRenderer;

    private ScoreManager scoreManager;

    private StateManager stateManager;

    //Initialises the variables
    void Start()
    {
        player = this.gameObject;
        currentPos = player.transform.position;
        tweener = GetComponent<Tweener>();
        lastInput = Direction.Null;
        currentInput = Direction.Null;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementParticles = GetComponent<ParticleSystem>();
        colliding = false;
        collisionSpawnPos = currentPos;

        scoreManager = GameObject.FindWithTag("GameController").GetComponent<ScoreManager>();
        stateManager = GameObject.FindWithTag("GameController").GetComponent<StateManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (stateManager.getState() == StateManager.GameState.Normal ||
            stateManager.getState() == StateManager.GameState.Scared ||
            stateManager.getState() == StateManager.GameState.Recovering) 
        
        {
            //Gets input from player
            lastInput = getDirection();

            //If the gameobject is not currently moving
            if (tweener.getActiveTween() == null)
            {
                //If the player can travel in the direction inputted
                if (checkDirection(lastInput) == "Walkable")
                {
                    colliding = false;
                    currentInput = lastInput;
                    movePlayer(lastInput);                              //move player in this direction
                    animator.SetInteger("Direction", (int)lastInput);   //set animator to match state of direction
                    if (!walkingSound.isPlaying)
                    {
                        walkingSound.Play();                             //play the audio source
                    }
                    movementParticles.Play();                           //play the particle effect
                }
                //Else if the player can travel in the direction they are currently travelling
                else if (checkDirection(currentInput) == "Walkable")
                {
                    colliding = false;
                    movePlayer(currentInput);                               //keep moving player in current direction
                    animator.SetInteger("Direction", (int)currentInput);    //set animator to match state of direction
                    if (!walkingSound.isPlaying)
                    {
                        walkingSound.Play();                                 //play the audio source
                    }
                    movementParticles.Play();                              //play the particle effect
                }
                else if (checkDirection(currentInput) == "Teleporter")
                {
                    teleportPlayer();
                    Debug.Log("Reached Teleporter");
                }
                else
                {
                    if (colliding == false && started == true)
                    {
                        collisionEffects(currentInput);
                        colliding = true;
                    }
                    animator.SetInteger("Direction", (int)Direction.Null);
                }

            }
        }
    }


    //Sets lastInput to a direction that the player has given it
    private Direction getDirection()
    {
        //Update current position
        currentPos = player.transform.position;

        if (Input.GetKeyDown(KeyCode.W))
        {
            //Return up if the player presses W
            return Direction.Up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //Return left if the player presses W
            return Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //Return down if the player presses W
            return Direction.Down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Return right if the player presses W
            return Direction.Right;
        }

        //Return current value of lastInput if the player did not provide input
        return lastInput;
    }


    //Moves the player in the given direction
    private void movePlayer(Direction direction)
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
        else {
            //nextPos set to player's current position
            nextPos = currentPos;
        }

        started = true;
        collisionSpawnPos = nextPos;

        //Tween player in direction of nextPos from currentPos
        tweener.AddTween(player.transform, currentPos, nextPos, 0.1f * Time.deltaTime);
    }


    //Used to check if the player can travel in the given direction
    private string checkDirection(Direction direction) {

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
        else {
            nextPos = Vector3.zero;
        }

        if(nextPos.x <= 0) {nextPos.x = 0;} else if(nextPos.x >= LevelGenerator.getColumns() - 1){nextPos.x = LevelGenerator.getColumns() - 1;}
        
        if(nextPos.y <= 0) {nextPos.x = 0;} else if(nextPos.y >= LevelGenerator.getRows() - 1) {nextPos.y = LevelGenerator.getRows() - 1; }


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

    void OnCollisionEnter(Collision other) {
      if(started == true)
      {
            if (other.gameObject.tag == "Pellet")
            {
                if (!eatPellet.isPlaying)
                {
                    eatPellet.Play();
                }
                Destroy(other.gameObject);
                scoreManager.AddScore(10);
            }

            if (other.gameObject.tag == "Cherry")
            {
                Destroy(other.gameObject);
                scoreManager.AddScore(100);
            }

            if (other.gameObject.tag == "PowerPellet")
            {
                stateManager.setState(StateManager.GameState.Scared);
                Destroy(other.gameObject);
            }
      }  
        

        if (other.gameObject.tag == "Ghost" && stateManager.getState() == StateManager.GameState.Normal)
        {
            scoreManager.LoseLives();
            Instantiate(deathParticles, player.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            gameObject.SetActive(true);
        }
    }



    private void collisionEffects(Direction direction)
    {
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

        Instantiate(collisionParticles, nextPos, Quaternion.identity);
        collisionSound.Play();
    }


    private void teleportPlayer() {
        
        if(player.transform.position.x == 1)
        {
            gameObject.transform.position = new Vector3(26f, 14f, 0f);
        }
        else if(player.transform.position.x == 26)
        {
            gameObject.transform.position = new Vector3(1f, 14f, 0f);
        }
    }
}