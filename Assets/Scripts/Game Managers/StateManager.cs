using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public enum GameState { Awake, Normal, Scared, Recovering, OneDeadGhost, GameOver};
    GameState gameState;

    private float scaredTimer = 10f;
    private bool scaredMusicTrigger = false;
    private bool normalMusicTrigger = false;
    public Text scaredUI;

    private MusicController bgmController;
    private ScoreManager scoreManager;
    private SaveManager saveManager;
    public GUIManager guiManager;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        gameState = GameState.Awake;
        bgmController= gameObject.GetComponent<MusicController>();
        scoreManager = GetComponent<ScoreManager>();
        scaredUI.enabled = false;
        saveManager = GetComponent<SaveManager>();
        guiManager = GetComponent<GUIManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.Scared && scaredTimer > 3f) 
        {
            if (scaredMusicTrigger == false) 
            {
                bgmController.changeTrack(gameState);
                scaredMusicTrigger = true;
                normalMusicTrigger = false; ;
            }
            scaredTimer -= Time.deltaTime;
            scaredUI.enabled = true;
            scaredUI.text = "Scared! " + Mathf.Round(scaredTimer);
        }

        if(gameState == GameState.Scared && scaredTimer <= 3f) 
        {
            gameState = GameState.Recovering;
        }

        if(gameState == GameState.Recovering && scaredTimer > 0f)
        {
            scaredTimer -= Time.deltaTime;
            scaredUI.text = "Scared! " + Mathf.Round(scaredTimer);
        }

        if (gameState == GameState.Recovering && scaredTimer <= 0f)
        {
            scaredUI.enabled = false;
            gameState = GameState.Normal;
            scaredTimer = 10f;
            //Debug.Log(gameState);
        }

        if (gameState == GameState.Normal)
        {
            if(normalMusicTrigger == false) 
            {
                bgmController.changeTrack(gameState);
                normalMusicTrigger = true;
                scaredMusicTrigger = false;
            }
        }

        if(scoreManager.getLivesRemaining() == 0 || scoreManager.getPelletsRemaining() == 0) 
        {
            gameState = GameState.GameOver;
            saveManager.saveData(scoreManager.getScore(), guiManager.getTime(), guiManager.getTimeAsString());
        }
    }

    public void setState(GameState newState)
    {
        if(newState == GameState.Scared) 
        {
            scaredTimer = 10f;
        }
        gameState = newState;
    }

    public GameState getState()
    {
        return gameState;
    }
}
