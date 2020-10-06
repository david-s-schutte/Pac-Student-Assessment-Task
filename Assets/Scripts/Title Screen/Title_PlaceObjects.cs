using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to position the pacstudent and ghost sprites within the title screen

public class Title_PlaceObjects : MonoBehaviour
{
    private Vector3 spawnpos; 
    private Tweener tweener;
    private Animator animator;
    public string animationState;

    public Vector3 endpos = new Vector3(10f, -2f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(animationState);
        animator.speed = 0f;

        spawnpos = transform.position;
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
