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
        countdownTime -= Time.deltaTime;

        if(countdownTime <= 4f && countdownTime >= 3f)
        {
            countdown3.enabled = false;
            countdown2.enabled = true;
        }

        if (countdownTime <= 3f && countdownTime >= 2f) 
        {
            countdown2.enabled = false;
            countdown1.enabled = true;
        }

        if (countdownTime <= 2f && countdownTime >= 1f)
        {
            countdown1.enabled = false;
            countdownGo.enabled = true;
        }

        if (countdownTime <= 1f && countdownTime >= 0f)
        {
            countdownGo.enabled = false;
            startTime = Time.time;
        }

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
        }

        if(stateManager.getState() == StateManager.GameState.GameOver && gameoverTimer <= 3f) 
        {
            gameOver.enabled = true;
            gameoverTimer -= Time.deltaTime;
            Debug.Log(gameoverTimer);
        }
        if (gameoverTimer <= 0f)
        {
            SceneManager.LoadScene("StartScene");
        }
    }

    public void onClickChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void onClickQuitGame() 
    {
        Application.Quit();
    }
}
