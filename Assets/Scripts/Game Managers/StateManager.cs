using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    //Enumerator for the different gamestates
    public enum GameState { Awake, Normal, Scared, Recovering, OneDeadGhost, GameOver};
    GameState gameState;

    //Variables that control the scared state of the ghosts
    private float scaredTimer = 10f;
    private bool scaredMusicTrigger = false;
    private bool normalMusicTrigger = false;
    public Text scaredUI;

    //Variables that call functions of other scripts
    private MusicController bgmController;
    private ScoreManager scoreManager;
    private SaveManager saveManager;
    public GUIManager guiManager;

    //Variable that sets the status of the player's death
    public bool playerIsDead = false;
    public GameObject player;


    void Awake()
    {
        //PlayerPrefs.DeleteAll();              //Used for debugging purposes
        gameState = GameState.Awake;
        scaredUI.enabled = false;

        bgmController = GetComponent<MusicController>();
        scoreManager = GetComponent<ScoreManager>();
        saveManager = GetComponent<SaveManager>();
        guiManager = GetComponent<GUIManager>();
    }


    // Update is called once per frame
    void Update()
    {
        //If the gamestate is scared and there are more than three seconds left on the timer
        if(gameState == GameState.Scared && scaredTimer >= 3f) 
        {
            //Change the music to scared music if it isn't already playing it
            if (scaredMusicTrigger == false) 
            {
                bgmController.changeTrack(gameState);
                scaredMusicTrigger = true;
                normalMusicTrigger = false; ;
            }
            //Subtract time from the timer
            scaredTimer -= Time.deltaTime;
            //Enable and display time remaining
            scaredUI.enabled = true;
            scaredUI.text = "Scared! " + Mathf.Round(scaredTimer);
        }

        //If the gamestate is scared and there are less than three seconds left on the timer
        if(gameState == GameState.Scared && scaredTimer <= 3f) 
        {
            //Change the gamestate to recovering
            gameState = GameState.Recovering;
        }

        //If the gamestate is recovering and there are more than zero seconds on the timer
        if(gameState == GameState.Recovering && scaredTimer > 0f)
        {
            //Subtract time from the timer
            scaredTimer -= Time.deltaTime;
            //Display time remaining
            scaredUI.text = "Scared! " + Mathf.Round(scaredTimer);
        }

        //If the gamestate is recovering and the timer is less than zero
        if (gameState == GameState.Recovering && scaredTimer <= 0f)
        {
            //Disable the scared timer
            scaredUI.enabled = false;
            //Change the gamestate to normal
            gameState = GameState.Normal;
            //Reset the scared timer
            scaredTimer = 10f;
        }

        //If the gamestate is normal
        if (gameState == GameState.Normal)
        {
            //If the normalplay music isn't playing already, play it
            if(normalMusicTrigger == false) 
            {
                bgmController.changeTrack(gameState);
                normalMusicTrigger = true;
                scaredMusicTrigger = false;
            }
        }

        //If the player has run out of lives or if the player has collected all the pellets
        if(scoreManager.getLivesRemaining() == 0 || scoreManager.getPelletsRemaining() == 0) 
        {
            //Change the game state
            gameState = GameState.GameOver;
            //Save the player's score and time if required
            saveManager.saveData(scoreManager.getScore(), guiManager.getTime(), guiManager.getTimeAsString());
        }
    }


    //Setter for gameState
    public void setState(GameState newState)
    {
        if(newState == GameState.Scared) 
        {
            scaredTimer = 10f;
        }
        gameState = newState;
    }


    //Getter for gameState
    public GameState getState()
    {
        return gameState;
    }

    //Setter for playerIsDead
    public void setPlayerDeath(bool status) 
    {
        playerIsDead = status;
    }

    //Returns true if the player is dead
    public bool isPlayerDead() 
    {
        return playerIsDead;
    }
}
