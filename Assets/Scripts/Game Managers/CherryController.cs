using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherry;
    public Tweener cherryTweener;
    private int cherryDirection;
    private Vector3 startPos;
    private Vector3 endPos;
    public float duration = 5f;
    private bool firstSpawn;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnCherry", 30f, 30f);
        endPos = Vector3.zero;
        firstSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Time: " + Mathf.Round(Time.time) + " cherryDirection: " + cherryDirection);
        cherryDirection = Random.Range(0, 8);  
    }

    private void SpawnCherry() 
    {
        if (firstSpawn == false)
        {
            if (GameObject.FindWithTag("Cherry").transform.position == endPos)
            {
                Destroy(GameObject.FindWithTag("Cherry"));
            }
        }
        else
        {
            firstSpawn = false;
        }

        //Determines start and end pos depending on integer held by cherryDirection
        switch (cherryDirection)
        {
            case 0: //Top left to bottom right
                startPos = new Vector3(-16f, 32f, 0f); 
                endPos = new Vector3(44f, -2f, 0f); 
                break;
            case 1: //Left to right
                startPos = new Vector3(-16f, 14f, 0f); 
                endPos = new Vector3(44f, 14f, 0f); 
                break;
            case 2: //Bottom left to Top right
                startPos = new Vector3(-16f, -2f, 0f); 
                endPos = new Vector3(44f, 32f, 0f); 
                break;
            case 3: //Bottom to top
                startPos = new Vector3(13.5f, -2f, 0f); 
                endPos = new Vector3(13.5f, 32f, 0f); 
                break;
            case 4: //Top right to bottom left
                startPos = new Vector3(44f, 32f, 0f); 
                endPos = new Vector3(-16f, -2f, 0f); 
                break;
            case 5: //Right to left
                startPos = new Vector3(44f, 14f, 0f); 
                endPos = new Vector3(-16f, 14f, 0f); 
                break;
            case 6: //Bottom right to top left
                startPos = new Vector3(44f, - 2f, 0f); 
                endPos = new Vector3(-16f, 32f, 0f); 
                break;
            case 7: //Top to bottom
                startPos = new Vector3(13.5f, 32f, 0f); 
                endPos = new Vector3(13.5f, -2f, 0f); 
                break;
        }

        //Spawns the cherry
        Instantiate(cherry, startPos, Quaternion.identity);
        var cherryTweener = GameObject.FindWithTag("Cherry").GetComponent<Tweener>();

        if (cherryTweener.getActiveTween() == null) {
            Debug.Log("im null");
            cherryTweener.AddTween(cherry.transform, startPos, endPos, duration);
        }
        else
        {
            Debug.Log("im full of shit");
        }
        

        //Tweens the cherry
        
        //Debug.Log(cherryTweener.getActiveTweenAsString());
        //cherry.AddComponent<MoveCherry>();
        
    }
}
