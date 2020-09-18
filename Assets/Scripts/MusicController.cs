using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource currentBGM;
    public AudioClip start;
    public AudioClip normalPlay;
    public AudioClip powerPellet;
    public AudioClip oneDeadGhost;

    private float timer = 0.0f;
    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        currentBGM = GetComponent<AudioSource>();
        currentBGM.PlayOneShot(start, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > start.length && started == false)
        {
            currentBGM.Play();
            started = true;
        }
    }
}
