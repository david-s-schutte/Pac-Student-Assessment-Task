using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherry;
    private float timeInterval = 30f;
    private Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= 30f && Time.time % timeInterval == 0) 
        {
            SpawnCherry();
            //tweener.AddTween(cherry.transform, cherry.transform.position, new Vector3(32f, 14f, -1f), 5f);
            Debug.Log("spawned");
        }

        //Debug.Log(Time.time);

        //if(GameObject.FindWithTag("Cherry").transform.position == new Vector3(32f, 14f, -1f))
        //{
        //    Destroy(GameObject.FindWithTag("Cherry"));
        //}
    }

    private void SpawnCherry() 
    {
        Instantiate(cherry, new Vector3(-16f, 14f, -1f), Quaternion.identity);
        //new_cherry.name = "Bonus Cherry";
        
    }
}
