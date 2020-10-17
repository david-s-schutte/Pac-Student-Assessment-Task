using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMover : MonoBehaviour
{
    //Reference to the controller attached to the GameManager - used get endPos and startTime
    private CherryController controller;

    //Variables used in the calculation of the cherry's positon 
    public float duration = 50f;
    private Vector3 endPos;
    private float startTime;

    //Called before start
    void Awake() {
        controller = GameObject.FindWithTag("GameController").GetComponent<CherryController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        endPos = controller.getEndPos();
        startTime = controller.getStartTime();
    }

    // Update is called once per frame
    void Update()
    {
        //The cherry is lerped within this script because the Tweener script refused to work with it
        if(Vector3.Distance(gameObject.transform.position, endPos) > 0.1f)
        {
            float timeFraction = ((Time.time - startTime) / duration) * Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPos, timeFraction);
        }
        
        if(Vector3.Distance(gameObject.transform.position, endPos) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
