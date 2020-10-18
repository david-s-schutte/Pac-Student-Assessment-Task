using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenGUIManager : MonoBehaviour
{
    public Text highScore;
    public Text bestTime;

    void Start() 
    {
        highScore.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
        
        if (PlayerPrefs.GetString("BestTimeAsString") == "")
        {
            bestTime.text = "Best Time: 00:00:00";
        }
        else
        {
            bestTime.text = PlayerPrefs.GetString("BestTimeAsString");
        }

        //bestTime.text = "Best Time: " + PlayerPrefs.GetFloat("BestTime");


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
