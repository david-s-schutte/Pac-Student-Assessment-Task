﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationMusicController : MonoBehaviour
{
    //Variables used for controlling music
    AudioSource currentBGM;
    public AudioClip start;         //Start Jingle
    public AudioClip normalPlay;    //Normal Play

    //Variables used for the start jingle
    private float timer = 0.0f;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        currentBGM = GetComponent<AudioSource>();
        currentBGM.PlayOneShot(start, 0.5f);    //Plays start jingle
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > start.length && started == false)    //When the start jingle has finished
        {
            currentBGM.Play();
            started = true;
        }
    }
}
