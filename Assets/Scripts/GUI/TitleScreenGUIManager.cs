using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenGUIManager : MonoBehaviour
{
    //Variables used to display scores and times
    public Text highScore;
    public Text bestTime;

    void Start() 
    {
        //Displays highest score reached
        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
        
        //If the best time as a string is blank
        if (PlayerPrefs.GetString("BestTimeAsString") == "")
        {
            bestTime.text = "Best Time: 00:00:00";
        }
        //If the best time has text
        else
        {
            bestTime.text = "Best " + PlayerPrefs.GetString("BestTimeAsString");
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
