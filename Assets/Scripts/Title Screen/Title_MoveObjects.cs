using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to move the pacstudent and ghost sprites on the title screen

public class Title_MoveObjects : MonoBehaviour
{
    private Vector3 spawnpos; 
    private Tweener tweener;

    public Vector3 endpos = new Vector3(10f, -2f, 0f);
    private float duration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        spawnpos = transform.position;
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= endpos.x)
        {
            transform.Translate(duration *Time.deltaTime, 0f, 0f);
        }
        else if (transform.position.x >= endpos.x)
        {
            transform.position = spawnpos;
        }
    }
}
