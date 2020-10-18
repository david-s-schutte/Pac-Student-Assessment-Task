using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string highScoreKey = "HighScore";
    private string bestTimeKey = "BestTime";
    private string bestTimeAsStringKey = "BestTimeAsString";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getHSKey() { return highScoreKey; }

    public string getBTKey() { return bestTimeKey; }

    public void saveData(int playerScore, float playerTime, string playerTimeString) {
        
        int currentHighScore = PlayerPrefs.GetInt(highScoreKey);
        float currentBestTime = PlayerPrefs.GetFloat(bestTimeKey);

        if (currentBestTime > playerTime || currentBestTime == 0f)
        {
            PlayerPrefs.SetFloat(bestTimeKey, playerTime);
            PlayerPrefs.SetString(bestTimeAsStringKey, playerTimeString);
        }

        if (currentHighScore < playerScore) 
        {
            PlayerPrefs.SetInt(highScoreKey, playerScore);
        }

        PlayerPrefs.Save();
    }
}
