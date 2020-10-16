using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMover : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    public float duration = 5f;
    private Tweener tweener;

    public CherryMover(Vector3 generatedStartPos, Vector3 generatedEndPos)
    {
        startPos = generatedStartPos;
        endPos = generatedEndPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tweener.getActiveTween() == null)
        {
            tweener.AddTween(gameObject.transform, startPos, endPos, duration);
        }
    }
}
