﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCorner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0f, 0f, 30f * Time.deltaTime);
    }
}