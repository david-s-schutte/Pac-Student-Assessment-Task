using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int lives;
    public int pelletsRemaining;

    public Text scoreDisplay;
    public Text lifeCount;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        lives = 3;
        scoreDisplay.text = "Score: " + score;
        lifeCount.text = "" + lives;
        pelletsRemaining = LevelGenerator.getPelletCount();
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + score;
        lifeCount.text = "" + lives;
    }

    public void AddScore(int adder)
    {
        score += adder;
        if(adder == 10) 
        {
            pelletsRemaining--;
        }
    }

    public void LoseLives() 
    {
        lives--;
    }

    public int getLivesRemaining() 
    {
        return lives;
    }

    public int getPelletsRemaining() 
    {
        return pelletsRemaining;
    }
}
