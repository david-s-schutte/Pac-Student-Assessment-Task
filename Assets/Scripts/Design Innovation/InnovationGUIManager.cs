using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InnovationGUIManager : MonoBehaviour
{
    //Countdown Timer variables
    public float countdownTime = 5f;
    public Image countdown3;
    public Image countdown2;
    public Image countdown1;
    public Image countdownGo;

    //Timer variables
    public Text ghostsRemaining;
    public Text livesRemaining;

    //GameOver variables
    public Image gameOver;
    private float gameoverTimer = 3f;

    private InnovationScoreManager scoreManager;
    //int livesRemaining = 3;

    void Start()
    {
        countdown3.enabled = true;
        countdown2.enabled = false;
        countdown1.enabled = false;
        countdownGo.enabled = false;
        gameOver.enabled = false;
        scoreManager = GetComponent<InnovationScoreManager>();
    }

    void Update()
    {
        //Subtracts time from countdown timer
        countdownTime -= Time.deltaTime;

        //Enables '3'
        if (countdownTime <= 4f && countdownTime >= 3f)
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
            //startTime = Time.time;
        }
        //After countdown has ended
        if (countdownTime <= 0f)
        {
            countdownTime = 0f;
            ghostsRemaining.text = "Ghosts Remaining: " + scoreManager.getGhostsRemaining();
            livesRemaining.text = "" + scoreManager.getLives();
        }

        if (scoreManager.getLives() == 0 || scoreManager.getGhostsRemaining() == 0 && gameoverTimer <= 3f)
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


    //Changes the scene depending on the scene name given
    public void onClickChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Closes the game
    public void onClickQuitGame()
    {
        Application.Quit();
    }
}
