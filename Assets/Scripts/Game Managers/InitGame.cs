﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    //To be used in Assessment Task 4
    public GameObject player;
    public GameObject ghost1Bobo;
    public GameObject ghost2Molars;
    public GameObject ghost3Robert;
    public GameObject ghost4Frederick;

    public float fastForward = 1f;
    private bool playerisDead = false;

    private StateManager stateManager;

    // Start is called before the first frame update
    void Start()
    {
        playerisDead = false;
        stateManager = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindWithTag("Player") == null && playerisDead == false) 
        {
            playerisDead = true;
            Invoke("respawnPlayer", 3.5f);
        }
    }

    private void respawnPlayer() 
    {
        Instantiate(player, new Vector3(1f, 27f, 0f), Quaternion.identity);
        playerisDead = false;
        stateManager.setPlayerDeath(false);
    }
}
