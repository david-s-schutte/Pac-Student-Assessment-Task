using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum GameState { Awake, Normal, Scared, Recovering, GameOver };
    GameState gameState;

    private MusicController bgmController;
    // Start is called before the first frame update
    void Awake()
    {
        gameState = GameState.Awake;
        bgmController= gameObject.GetComponent<MusicController>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameState == GameState.Scared) 
        {
            bgmController.changeTrack(gameState);
        }

        if (gameState == GameState.Normal)
        {
            bgmController.changeTrack(gameState);
        }
    }

    public void setState(GameState newState)
    {
        gameState = newState;
    }

    public GameState getState()
    {
        return gameState;
    }
}
