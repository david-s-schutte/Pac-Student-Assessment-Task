using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationScoreManager : MonoBehaviour
{
    private int lives = 3;
    private int ghostsRemaining = 4;

    public int getLives()
    {
        return lives;
    }

    public int getGhostsRemaining()
    {
        return ghostsRemaining;
    }

    public void killGhost() 
    {
        ghostsRemaining--;
    }

    public void loseLife() 
    {
        lives--;
    }
}
