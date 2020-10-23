using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationPacStudentController : MonoBehaviour
{
    private enum Direction { Up, Left, Down, Right, Null };     //Enum used to determine which direction the player has inputted
        Direction currentInput;

    public float speed = 4f;

    private ParticleSystem movementParticles;                   //Reference to particle system attached to gameobject
    private Animator animator;                                  //Reference to animator attached to gameobject
    public AudioSource grabToken;                               //Reference to audio source system attached to gameobject
    public AudioSource walkingSound;                            //Reference to the walking sound audioclip
    public AudioSource deathSound;                          //Reference to the collision sound audioclip                           
    public GameObject collisionParticles;                       //Reference to particle system for wall collisions
    public GameObject deathParticles;                           //Reference to the particle system for when the player dies
    private bool colliding;                                     //Boolean that prevents repeating of collision feedback
    private Vector3 collisionSpawnPos;
    private GameObject flashlight;
    private Collider myCollider;
    private SpriteRenderer sprite;

    private InnovationScoreManager scoreManager;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentInput = Direction.Null;
        animator = GetComponent<Animator>();
        movementParticles = GetComponent<ParticleSystem>();
        scoreManager = GameObject.FindWithTag("GameController").GetComponent<InnovationScoreManager>();
        flashlight = gameObject.transform.Find("FlashLight").gameObject;
        myCollider = GetComponent<Collider>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            walkingSound.pitch = 1.2f;
            speed = 6f;
        }
        else
        {
            walkingSound.pitch = 1f;
            speed = 4f;
        }
        if(isDead == false) 
        {
            movePlayer();
            determineAnimation();
        }
    }

    private void movePlayer() 
    {
        if (Input.GetKey(KeyCode.W))
        {
            //up if the player presses W
            currentInput = Direction.Up;
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, 0f);
            flashlight.transform.eulerAngles = new Vector3(0f, 0f, 90f);
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //left if the player presses A
            currentInput = Direction.Left;
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, 0f);
            flashlight.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //down if the player presses S
            currentInput = Direction.Down;
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, 0f);
            flashlight.transform.eulerAngles = new Vector3(0f, 0f, -90f);
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //right if the player presses D
            currentInput = Direction.Right;
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 0f);
            flashlight.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else 
        {
            currentInput = Direction.Null;
        }
    }

    private void determineAnimation() 
    {
        animator.SetInteger("Direction", (int)currentInput);    
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Pellet") 
        {
            Destroy(other.gameObject);
            grabToken.Play();
            scoreManager.killGhost();
        }

        if(other.gameObject.tag == "Ghost" && isDead == false) 
        {
            killPlayer();
            if(scoreManager.getLives() > 0) 
            {
                Invoke("respawn", 2f);
            }
            
        }
    }

    void respawn() 
    {
        isDead = false;
        myCollider.enabled = true;
        sprite.enabled = true;
        flashlight.SetActive(true);
        transform.position = Vector3.zero;
    }

    void killPlayer() 
    {
        isDead = true;
        myCollider.enabled = false;
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        sprite.enabled = false;
        scoreManager.loseLife();
        flashlight.SetActive(false);
        deathSound.Play();
    }
}
