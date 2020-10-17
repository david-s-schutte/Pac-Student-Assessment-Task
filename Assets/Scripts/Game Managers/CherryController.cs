using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherry;               //Reference to Cherry prefab
    
    private int cherryDirection;            //Stores the direction the cherry will travel in
    private Vector3 startPos;               //Stores the starting position of the cherry
    private Vector3 endPos;                 //Stores the end position of the cherry
    private float startTime;                //Stores the time in which cherry was instantiated

    // Start is called before the first frame update
    void Start()
    {
        //Calls SpawnCherry() after 30 seconds, then calls it every 30 seconds
        InvokeRepeating("SpawnCherry", 30f, 30f);
    }

    //Used to spawn the cherry
    private void SpawnCherry() 
    {
        //Generates a "direction" via index. Refer to the below switch case to see which integer references which direction
        cherryDirection = Random.Range(0, 4);

        //Determines start and end pos depending on integer held by cherryDirection
        switch (cherryDirection)
        {
            case 0: //Left to right
                startPos = new Vector3(-20f, 14f, 0f); 
                endPos = new Vector3(60f, 14f, 0f); 
                break;
            case 1: //Bottom to top
                startPos = new Vector3(13.5f, -10f, 0f); 
                endPos = new Vector3(13.5f, 50f, 0f); 
                break;
            case 2: //Right to left
                startPos = new Vector3(50f, 14f, 0f); 
                endPos = new Vector3(-30f, 14f, 0f); 
                break;
            case 3: //Top to bottom
                startPos = new Vector3(13.5f, 40f, 0f); 
                endPos = new Vector3(13.5f, -20f, 0f); 
                break;
        }

        //sets the start time of when the cherry was created
        startTime = Time.time;

        //Spawns the cherry
        Instantiate(cherry, startPos, Quaternion.identity);
        
    }


    //Getter for the endPos Vector3
    public Vector3 getEndPos()
    {
        return endPos;
    }


    //Getter for the startTime float
    public float getStartTime()
    {
        return startTime;
    }
}
