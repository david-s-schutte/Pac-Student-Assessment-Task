using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;

    public Text scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreDisplay.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplay.text = "Score: " + score;
    }

    public void AddScore(int adder)
    {
        score += adder;
    }
}
