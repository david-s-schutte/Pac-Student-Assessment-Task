using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Vector3 currentPos;
    private Vector3 nextPos;
    private Animator animator;
    private GameObject player;
    private Tweener tweener;
    private enum Direction {Up, Left, Down, Right, Null};
    Direction lastInput;
    Direction currentInput;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        player = this.gameObject;
        animator = GetComponent<Animator>();
        currentPos = player.transform.position;
        nextPos = Vector3.zero;
        lastInput = Direction.Null;
        currentInput = Direction.Null;
    }

    // Update is called once per frame
    void Update()
    {
        //tweener.AddTween(player.transform, currentPos, new Vector3(currentPos.x, currentPos.y+1, -1f), 1f * Time.deltaTime);

        currentInput = getDirection();

        if (tweener.getActiveTween() == null) {
            currentPos = player.transform.position;
            movePlayer(currentInput);
            determineAnimation(currentInput);
        }
        
        if (lastInput != currentInput) {
            lastInput = currentInput;
        }

        Debug.Log(lastInput);
    }

    private Direction getDirection() 
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            return Direction.Up;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            return Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            return Direction.Down;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            return Direction.Right;
        }

        return lastInput;
    }

    private void movePlayer(Direction direction) 
    {
        if (direction == Direction.Up) 
        {
            nextPos = new Vector3(currentPos.x, currentPos.y + 1, -1f);
        }
        if (direction == Direction.Left)
        {
            nextPos = new Vector3(currentPos.x - 1, currentPos.y, -1f);
        }
        if (direction == Direction.Down) 
        {
            nextPos = new Vector3(currentPos.x, currentPos.y - 1, -1f);
        }
        if (direction == Direction.Right) 
        {
            nextPos = new Vector3(currentPos.x + 1, currentPos.y, -1f);
        }

        Debug.Log(LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y));

        if (LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 0 || 
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 5 || 
            LevelGenerator.getCoordinates((int)nextPos.x, (int)nextPos.y) == 6) 
        {
            tweener.AddTween(player.transform, currentPos, nextPos, 0.2f * Time.deltaTime);
            
        }
       
    }

    private void determineAnimation(Direction direction) 
    {
        

        if (direction == Direction.Up)
        {
            animator.SetBool("movingUp", true);

            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", false);
            animator.SetBool("movingDown", false);
        }
        if (direction == Direction.Left)
        {
            animator.SetBool("movingLeft", true);

            animator.SetBool("movingRight", false);
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
        }
        if (direction == Direction.Down)
        {
            animator.SetBool("movingDown", true);

            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", false);
            animator.SetBool("movingUp", false);
        }
        if (direction == Direction.Right)
        {
            animator.SetBool("movingRight", true);

            animator.SetBool("movingLeft", false);
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
        }
    }
}
