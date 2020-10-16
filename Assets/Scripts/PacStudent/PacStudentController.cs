using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
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


    [SerializeField] private GameObject empty;

    //Initialises the variables
    void Start()
    {
        player = this.gameObject;
        currentPos = player.transform.position;
        tweener = GetComponent<Tweener>();
        lastInput = Direction.Null;
        currentInput = Direction.Null;

        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        movementParticles = GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        //Gets input from player
        lastInput = getDirection();

        //If the gameobject is not currently moving
        if (tweener.getActiveTween() == null)
        {
            //If the player can travel in the direction inputted
            if (isWalkable(lastInput)) {
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
            else if(isWalkable(currentInput)){
                movePlayer(currentInput);                               //keep moving player in current direction
                animator.SetInteger("Direction", (int)currentInput);    //set animator to match state of direction
                //audioSource.Play();
                if (!walkingSound.isPlaying)
                {
                    walkingSound.Play();                                 //play the audio source
                }
                movementParticles.Play();                              //play the particle effect
            }
            else {
                //movePlayer(Direction.Null);
                animator.SetInteger("Direction", (int)Direction.Null);
            }
            
        }

        //Debug.Log(audioSource.isPlaying);
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

        //Tween player in direction of nextPos from currentPos
        tweener.AddTween(player.transform, currentPos, nextPos, 0.2f * Time.deltaTime);
    }


    //Used to check if the player can travel in the given direction
    private bool isWalkable(Direction direction) {

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

        if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 0 ||   //If the nextPos coordinates don't contain anything
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 5 ||   //If the nextPos coordinates contain a pellet
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 6)     //If the nextPos coordinates contain a power pellet
            {
                //The player can travel in the given direction
                return true;
            }

        //The player can't travel in the given direction
        return false;
    }



    void OnCollisionEnter(Collision other) {
        
        if(other.gameObject.tag == "Pellet")
        {
            if (!eatPellet.isPlaying)
            {
                eatPellet.Play();
            }
            
        }
        else {
            //audioSource.clip = audioClips[0];
        }

        Debug.Log("Colliding with: " + other.gameObject.tag);
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Pellet")
        {
            
            Destroy(other.gameObject);
        }
    }
}