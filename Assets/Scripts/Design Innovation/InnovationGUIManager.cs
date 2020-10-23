using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InnovationGUIManager : MonoBehaviour
{
    //Timer variables
    public Text ghostsRemaining;
    public Text livesRemaining;

    //GameOver variables
    public Image gameOver;
    private float gameoverTimer = 3f;

    private InnovationScoreManager scoreManager;

    void Start()
    {
        gameOver.enabled = false;
        scoreManager = GetComponent<InnovationScoreManager>();
    }

    void Update()
    {
        ghostsRemaining.text = "Ghosts Remaining: " + scoreManager.getGhostsRemaining();
        livesRemaining.text = "" + scoreManager.getLives();

        //If the player has run out of lives or if all the tokens have been collected
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
}
