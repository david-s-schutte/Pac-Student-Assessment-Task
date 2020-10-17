using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float timer = 0.25f;

    void Awake()
    {
        Invoke("selfDestruct", timer);
    }

    void selfDestruct() {
        Destroy(gameObject);
    }
}
