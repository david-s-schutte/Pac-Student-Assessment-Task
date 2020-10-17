using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    private Tweener tweener;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        player = this.gameObject;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position == new Vector3(2f, 28f, -1f))
        {
            animator.SetBool("movingRight", true);
            animator.SetBool("movingLeft", false);
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(7f, 28f, -1f), 1.5f);
        }

        if (player.transform.position == new Vector3(7f, 28f, -1f))
        {
            animator.SetBool("movingDown", true);
            animator.SetBool("movingLeft", false);
            animator.SetBool("movingUp", false);
            animator.SetBool("movingRight", false);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(7f, 21f, -1f), 1.5f);
        }

        if (player.transform.position == new Vector3(7f, 21f, -1f))
        {
            animator.SetBool("movingLeft", true);
            animator.SetBool("movingRight", false);
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(2f, 21f, -1f), 1.5f);
        }

        if (player.transform.position == new Vector3(2f, 21f, -1f))
        {
            animator.SetBool("movingUp", true);
            animator.SetBool("movingLeft", false);
            animator.SetBool("movingRight", false);
            animator.SetBool("movingDown", false);
            tweener.AddTween(player.transform, player.transform.position, new Vector3(2f, 28f, -1f), 1.5f);
        }
    }
}
