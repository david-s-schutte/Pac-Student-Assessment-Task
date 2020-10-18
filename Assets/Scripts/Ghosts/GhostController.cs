using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private enum Direction { Up, Left, Down, Right, Null };

    public StateManager stateManager;
    public ScoreManager scoreManager;
    public MusicController musicController;

    private Animator animator;
    private bool killed = false;
    private float deadTimer = 5f;

    //public AnimatorController[] animationController = new AnimatorController[3];

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(killed == true) 
        {
            deadTimer -= Time.deltaTime;
            if(deadTimer <= 0f) 
            {
                killed = false;
                deadTimer = 5f;
                animator.SetBool("dead", false);
            }
        }

        if (stateManager.getState() == StateManager.GameState.Normal && !animator.GetBool("dead")) 
        {
            animator.SetBool("normalPlay", true);
            animator.SetInteger("direction", 2);

            animator.SetBool("scared", false);
            animator.SetBool("recovering", false);
        }

        else if (stateManager.getState() == StateManager.GameState.Scared && !animator.GetBool("dead"))
        {
            animator.SetBool("scared", true);
            animator.SetInteger("direction", 2);

            animator.SetBool("recovering", false);
            animator.SetBool("normalPlay", false);
        }

        else if (stateManager.getState() == StateManager.GameState.Recovering && !animator.GetBool("dead"))
        {
            animator.SetBool("recovering", true);
            animator.SetInteger("direction", 2);

            animator.SetBool("scared", false);
            animator.SetBool("normalPlay", false);
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player" && stateManager.getState() != StateManager.GameState.Normal) 
        {
            //Debug.Log("I, " + gameObject.name + ", am Dead.");
            killed = true;
            scoreManager.AddScore(300);
            musicController.changeTrack(StateManager.GameState.OneDeadGhost);
            animator.SetBool("dead", true);

            animator.SetBool("scared", false);
            animator.SetBool("recovering", false);
            animator.SetBool("normalPlay", false);
        }
    }
}
