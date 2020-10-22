using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    //Countdown Timer variables
    public float countdownTime = 5f;
    public Image countdown3;
    public Image countdown2;
    public Image countdown1;
    public Image countdownGo;

    //Timer variables
    public Text timer;
    private float startTime = 0f;

    //GameOver variables
    public Image gameOver;
    private StateManager stateManager;
    private float gameoverTimer = 3f;

    //Time variable to track the player's time
    private float totalTime;

    void Start() 
    {
        countdown3.enabled = true;
        countdown2.enabled = false;
        countdown1.enabled = false;
        countdownGo.enabled = false;
        gameOver.enabled = false;
        stateManager = GetComponent<StateManager>();
    }

    void Update() 
    {
        //Subtracts time from countdown timer
        countdownTime -= Time.deltaTime;

        //Enables '3'
        if(countdownTime <= 4f && countdownTime >= 3f)
        {
            countdown3.enabled = false;
            countdown2.enabled = true;
        }
        //Enables '2'
        if (countdownTime <= 3f && countdownTime >= 2f) 
        {
            countdown2.enabled = false;
            countdown1.enabled = true;
        }
        //Enables '1'
        if (countdownTime <= 2f && countdownTime >= 1f)
        {
            countdown1.enabled = false;
            countdownGo.enabled = true;
        }
        //Enables 'GO'
        if (countdownTime <= 1f && countdownTime >= 0f)
        {
            countdownGo.enabled = false;
            startTime = Time.time;
        }
        //After countdown has ended
        if(countdownTime <= 0f && stateManager.getState() != StateManager.GameState.GameOver) 
        {
            countdownTime = 0f;

            //Determines how many minutes have passed - rounds down to the smallest integer
            float min = Mathf.FloorToInt((Time.time - startTime) / 60);

            //Determines how many seconds have passed - rounds down to the smallest integer
            float sec = Mathf.FloorToInt((Time.time - startTime) % 60);

            //Determines how many milliseconds have passed
            float ms = ((Time.time - startTime) % 1) * 1000;

            timer.text = "Time: " + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec) + ":" + string.Format("{0:00}", ms);

            //Records time as float
            totalTime = Time.time - startTime;

        }

        //If the gameState in StateManager is GameOver and the gameoverTimer is less than or equal to 3 seconds
        if(stateManager.getState() == StateManager.GameState.GameOver && gameoverTimer <= 3f) 
        {
            //Enable gameover image
            gameOver.enabled = true;
            //Subtract time from the gameover timer
            gameoverTimer -= Time.deltaTime;
        }
        //If the gameoverTimer is less than 0 seconds
        if (gameoverTimer <= 0f)
        {
            //Return to start scene
            SceneManager.LoadScene("StartScene");
        }
    }

    //Changes scene depending on given name
    public void onClickChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Quits the game
    public void onClickQuitGame() 
    {
        Application.Quit();
    }

    //Getter for the time as a float
    public float getTime() { return totalTime; }

    //Getter for the time as a string
    public string getTimeAsString() { return timer.text; }
}
