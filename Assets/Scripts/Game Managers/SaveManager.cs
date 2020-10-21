using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string highScoreKey = "HighScore";
    private string bestTimeKey = "BestTime";
    private string bestTimeAsStringKey = "BestTimeAsString";
    private ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
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

        if (currentHighScore < playerScore) 
        {
            PlayerPrefs.SetInt(highScoreKey, playerScore);
        }

        if ((currentBestTime > playerTime || currentBestTime == 0f) && scoreManager.getPelletsRemaining() == 0)
        {
            PlayerPrefs.SetFloat(bestTimeKey, playerTime);
            PlayerPrefs.SetString(bestTimeAsStringKey, playerTimeString);
        }

        PlayerPrefs.Save();
    }
}
