using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    //Variables used for controlling music
    AudioSource currentBGM;
    public AudioClip start;         //Start Jingle
    public AudioClip normalPlay;    //Normal Play
    public AudioClip powerPellet;   //Power Pellet Music
    public AudioClip oneDeadGhost;  //When one ghost is dead

    //Variables used for the start jingle
    private float timer = 0.0f;
    private bool started = false;

    //Reference to the attached statemanager
    private StateManager stateManager;


    // Start is called before the first frame update
    void Start()
    {
        currentBGM = GetComponent<AudioSource>();
        stateManager = gameObject.GetComponent<StateManager>();
        currentBGM.PlayOneShot(start, 0.5f);    //Plays start jingle
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > start.length && started == false)    //When the start jingle has finished
        {
            currentBGM.Play();
            stateManager.setState(StateManager.GameState.Normal);
            started = true;

        }
    }


    //Changes music track when the game's state changes
    public void changeTrack(StateManager.GameState newState)
    { 
        currentBGM.Stop();
        //Determines the new clip to be played
        switch (newState)
        {
            case StateManager.GameState.Scared: currentBGM.clip = powerPellet; break;
            case StateManager.GameState.Normal: currentBGM.clip = normalPlay; break;
            case StateManager.GameState.OneDeadGhost: currentBGM.clip = oneDeadGhost; break;
        }
        currentBGM.Play();     
    }

    //Getter for the audio being played
    public AudioClip getCurrentTrack() 
    {
        return currentBGM.clip;
    }
}
