using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
    public InnovationGhostController ghostController;
    // Start is called before the first frame update
    void Start()
    {
        ghostController = GetComponentInParent<InnovationGhostController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player") 
        {
            ghostController.setChasePlayer(true);
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            ghostController.setChasePlayer(false);
        }
    }
}
