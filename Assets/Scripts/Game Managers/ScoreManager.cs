using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //Variables used for calculating score
    public int score;                           //Stores the player's score
    public int lives;                           //Stores the player's lives
    public int pelletsRemaining;                //Stores the amount of pellets remaining

    //Variables used for UI
    public Text scoreDisplay;                   //Reference to the score text
    public Text lifeCount;                      //Reference to the lives counter

    public AudioSource deathSound;              //Audio for when the player dies

    
    // Start is called before the first frame update
    void Start()
    {
        //Initialises variables
        score = 0;
        lives = 3;
        scoreDisplay.text = "Score: " + score;
        lifeCount.text = "" + lives;
        pelletsRemaining = LevelGenerator.getPelletCount();
        
    }

    // Update is called once per frame
    void Update()
    {
        //UI elements are updated every frame
        scoreDisplay.text = "Score: " + score;
        lifeCount.text = "" + lives;
    }


    //Adds points to the score variable
    public void AddScore(int adder)
    {
        //Adds the value passed to it to the score
        score += adder;
        //If the value passed is equal to 10 i.e. is a pellet
        if(adder == 10) 
        {
            //subtract one pellet from the amount remaining
            pelletsRemaining--;
        }
    }


    //Subtracts lives when the player dies
    public void LoseLives() 
    {
        deathSound.Play();
        lives--;
    }


    //Getter for the amount of lives the player has
    public int getLivesRemaining() 
    {
        return lives;
    }


    //Getter for the amount of pellets remaining
    public int getPelletsRemaining() 
    {
        return pelletsRemaining;
    }


    //Getter for the player's score
    public int getScore()
    {
        return score;
    }
}
