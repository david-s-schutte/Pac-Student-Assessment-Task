using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    //Used for respawning player
    public GameObject player;
    private bool playerisDead = false;
    private StateManager stateManager;

    //Used for debugging
    public float fastForward = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        playerisDead = false;
        stateManager = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is dead and the gameobject is missing
        if (GameObject.FindWithTag("Player") == null && playerisDead == false) 
        {
            playerisDead = true;
            //Respawn the player after waiting 3.5 seconds
            Invoke("respawnPlayer", 3.5f);
        }
    }

    //Respawns the player
    private void respawnPlayer() 
    {
        var respawnPlayer = Instantiate(player, new Vector3(1f, 27f, 0f), Quaternion.identity);
        respawnPlayer.name = "PacStudent";
        playerisDead = false;
        stateManager.setPlayerDeath(false);
    }
}
