using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string highScoreKey = "HighScore";                  //Key used in playerprefs for saving high score
    private string bestTimeKey = "BestTime";                    //Key used in playerprefs for saving the best time (as a float)
    private string bestTimeAsStringKey = "BestTimeAsString";    //Key used in playerprefs for saving the best time (as a string)
    private ScoreManager scoreManager;                          //Reference to the score manager

    // Start is called before the first frame update
    void Start()
    {
        //Gets the score manager attached to the gameobject
        scoreManager = GetComponent<ScoreManager>();
    }

    //Getter for the highest score key
    public string getHSKey() { return highScoreKey; }

    //Getter for the best time key value
    public string getBTKey() { return bestTimeKey; }


    //Gets the player's score and time and determines whether to save them or not
    public void saveData(int playerScore, float playerTime, string playerTimeString) 
    {
        //Stores the current high score and best time in temporary variables
        int currentHighScore = PlayerPrefs.GetInt(highScoreKey);
        float currentBestTime = PlayerPrefs.GetFloat(bestTimeKey);

        //If the player has a high score
        if (currentHighScore < playerScore) 
        {
            //Write it to playerprefs
            PlayerPrefs.SetInt(highScoreKey, playerScore);
        }

        //If the player has a lower time or the current best time is 0 AND they collected all the pellets
        if ((currentBestTime > playerTime || currentBestTime == 0f) && scoreManager.getPelletsRemaining() == 0)
        {
            //Writes the best time as a float to playerprefs
            PlayerPrefs.SetFloat(bestTimeKey, playerTime);
            //Writes the best time in string form to playerprefs
            PlayerPrefs.SetString(bestTimeAsStringKey, playerTimeString);
        }

        //Save all changes
        PlayerPrefs.Save();
    }
}
