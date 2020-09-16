using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int[,] levelMapQuad1;
    private int[,] levelMapQuad2;
    private int[,] levelMapQuad3;
    private int[,] levelMapQuad4;

    int columns, rows;

    public GameObject background, empty;
    public GameObject oCorner;
    public GameObject oWall;
    public GameObject iCorner;
    public GameObject iWall;
    public GameObject tJunct;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject bonusCherry;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(background);

        rows = 30;
        columns = 28;

        Quaternion cornerRotation = Quaternion.identity;

        levelMapQuad1 = new int[columns, rows];
        levelMapQuad2 = new int[columns, rows];
        levelMapQuad3 = new int[columns, rows];
        levelMapQuad4 = new int[columns, rows];

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                levelMapQuad1[c, r] = Random.Range(0, 8);
                SpawnTile(c, r, levelMapQuad1[c, r]);
                
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTile(int x, int y, int value)
    {
        Vector3 spawnPosition = new Vector3(x - (columns - 0.5f), y - (rows - 0.5f), 0f);
        switch (value)
        {
            //case 0: Instantiate(empty, spawnPosition, Quaternion.identity); break;
            case 1: Instantiate(oCorner, spawnPosition, Quaternion.identity); break;
            case 2: Instantiate(oWall, spawnPosition, Quaternion.identity); break;
            case 3: Instantiate(iCorner, spawnPosition, Quaternion.identity); break;
            case 4: Instantiate(iWall, spawnPosition, Quaternion.identity); break;
            case 5: Instantiate(pellet, spawnPosition, Quaternion.identity); break;
            case 6: Instantiate(powerPellet, spawnPosition, Quaternion.identity); break;
            case 7: Instantiate(tJunct, spawnPosition, Quaternion.identity); break;
        }
    }

    Vector3 determineRotation()
    {
        return new Vector3(0f, 0f, 90f);
    }
}
