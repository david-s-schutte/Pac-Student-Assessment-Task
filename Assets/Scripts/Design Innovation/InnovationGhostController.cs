using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationGhostController : MonoBehaviour
{
    private enum Direction { Up, Left, Down, Right, Null };  
    Direction currentInput;

    public GameObject player;
    public GameObject myToken;
    private bool chasePlayer = false;
    private Animator animator;
    private Collider myCollider;

    private int currentPatrolPoint = 0;
    public Transform[] patrolPoints = new Transform[5];

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myToken != null) 
        {
            if (chasePlayer == true)
            {
                followPlayer();
            }
            else 
            {
                followPatrolPoints();
            }
        }
        else 
        {
            killGhost();
        }
    }

    void followPlayer() 
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (distance) * Time.deltaTime);

        if(player.transform.position.y >= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Up); }
        else if (player.transform.position.y <= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Down); }
        else if (player.transform.position.x >= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Right); }
        else if (player.transform.position.x <= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Left); }
    }

    void followPatrolPoints() 
    {
        if(currentPatrolPoint < patrolPoints.Length - 1) 
        {
            if (patrolPoints[currentPatrolPoint + 1].position.y >= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Up); }
            else if (patrolPoints[currentPatrolPoint + 1].position.y <= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Down); }
            else if (patrolPoints[currentPatrolPoint + 1].position.x >= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Right); }
            else if (patrolPoints[currentPatrolPoint + 1].position.x <= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Left); }

            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentPatrolPoint + 1].position, 4f * Time.deltaTime);
        }
        else 
        {
            currentPatrolPoint = 0;
            if (patrolPoints[0].position.y >= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Up); }
            else if (patrolPoints[0].position.y <= transform.position.y) { animator.SetInteger("Direction", (int)Direction.Down); }
            else if (patrolPoints[0].position.x >= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Right); }
            else if (patrolPoints[0].position.x <= transform.position.x) { animator.SetInteger("Direction", (int)Direction.Left); }

            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[1].position, 4f * Time.deltaTime);
        }
    }

    public bool getChasePlayer() 
    {
        return chasePlayer;
    }

    public void setChasePlayer(bool newValue) 
    {
        chasePlayer = newValue;
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player") 
        {
            chasePlayer = false;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "PatrolPoint") 
        {
            currentPatrolPoint++;
        }
    }

    void killGhost() 
    {
        animator.SetBool("isDead", true);
        animator.SetInteger("Direction", 5);
        transform.eulerAngles = new Vector3(0f, 0f, 270f);
        myCollider.enabled = false;
    }
}