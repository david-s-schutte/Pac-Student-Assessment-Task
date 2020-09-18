using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{

    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();
    private List<Tween> temp = new List<Tween>(); //used to avoid invalid operation exception error

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tween activeTween in activeTweens)
        {
            if (activeTween != null)
            {
                temp.Add(activeTween);
            }
        }

        foreach(Tween activeTween in temp)
        {
            if (activeTween != null)
            {
                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
                {
                    float timeFraction = (Time.time - activeTween.StartTime) / activeTween.Duration;
                    //float TF = Mathf.Pow(timeFraction, 3f);
                    activeTween.Target.transform.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);
                }

                if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) <= 0.1f)
                {
                    activeTween.Target.position = activeTween.EndPos;
                    activeTweens.Remove(activeTween);
                }
            }
        }
    }
    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        bool exists = TweenExists(targetObject);
        if (exists == false)
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        else
        {
            return false;
        }
        //if (activeTween == null)
          //activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
    }

    public bool TweenExists(Transform Target)
    {
        bool exists = false;
        int i = 0;
        while(i < activeTweens.Count)
        {
            if (Target.transform == activeTweens[i].Target)
            {
                exists = true;
            }
            i++;
        }
        return exists;
    }
}
