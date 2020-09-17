using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    int columns = 28;
    int rows = 29;

    public GameObject oCorner;
    public GameObject oWall;
    public GameObject iCorner;
    public GameObject iWall;
    public GameObject tJunct;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject bonusCherry;
    private GameObject[,] gameObjects;

    int[,] levelMap = {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0,0,0,0,4,0,0,0,5,0,0,0,0,0,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0,0,0,0,4,0,3,3,5,1,2,2,2,2,2},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0,0,4,4,3,0,4,4,5,2,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0,0,0,0,0,0,4,4,5,2,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3,3,0,3,4,4,3,4,5,2,0,0,0,0,0},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4,4,0,3,4,4,3,4,5,1,2,2,2,2,1},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,4,4,5,5,5,5,5,5,2},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3,3,4,4,3,5,4,4,5,3,4,4,3,5,2},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4,4,4,4,3,5,3,3,5,3,4,4,3,5,2},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,2},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3,3,5,3,4,4,4,3,5,3,4,4,3,5,2},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4,4,5,4,0,0,0,4,5,4,0,0,4,6,2},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4,4,5,3,4,4,4,3,5,3,4,4,3,5,2},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4,4,5,5,5,5,5,5,5,5,5,5,5,5,2},
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7,7,2,2,2,2,2,2,2,2,2,2,2,2,1},
        };

    // Start is called before the first frame update
    void Start()
    {
        rows = levelMap.GetLength(1);
        columns = levelMap.GetLength(0);
        gameObjects = new GameObject[columns, rows];

        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                SpawnObject(c, r, levelMap[c, r]);
            }
        }

    }

    private void SpawnObject(int x, int y, int value)
    {
        Vector3 spawnPosition = new Vector3(-(rows - y), (columns - x));
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = determineRotation(x, y, value);

        switch (value)
        {
            case 1: gameObjects[x, y] = oCorner; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 2: gameObjects[x, y] = oWall; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 3: gameObjects[x, y] = iCorner; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 4: gameObjects[x, y] = iWall; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 5: gameObjects[x, y] = pellet; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 6: gameObjects[x, y] = powerPellet; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
            case 7: gameObjects[x, y] = tJunct; Instantiate(gameObjects[x, y], spawnPosition, rotation); break;
        }
    }

    Vector3 determineRotation(int x, int y, int value)
    {
        //oCorner
        if (value == 1) 
        {

            //Checking for oWall
            if (toLeft(x, y, 2)) //if there is an oWall to the left
            {

                if (toBottom(x, y, 2)) //if there is an oWall to the bottom
                {
                    return new Vector3(0f, 0f, 270f);
                }

                if (toTop(x, y, 2))
                {
                    return new Vector3(0f, 0f, 180f);
                }
            }
            if (toRight(x, y, 2))
            {
                if (toTop(x, y, 2))
                {
                    return new Vector3(0f, 0f, 90f);
                }
            }
        }

        //oWall
        if (value == 2) 
        {
            if((toRight(x, y, 2) || toLeft(x, y, 2)))
            {
                return new Vector3(0f, 0f, 90f);
            }
        }

        //iCorner
        if (value == 3)
        {
            //Between iWalls
            if (toLeft(x, y, 4) && toRight(x, y, 4))
            {
                if (toBottom(x, y, 4)) //Above an iWall
                {
                    //return new Vector3(0f, 0f, 90f);

                    if (toLeft(x, y - 1, 5))
                    {
                        return new Vector3(0f, 0f, 0f);
                    }

                    if (toRight(x, y + 1, 5))
                    {
                        return new Vector3(0f, 0f, 270f);
                    }
                }
                if (toBottom(x, y, 3)) //Above an iCorner
                {
                    if (toLeft(x, y - 1, 5))
                    {
                        return new Vector3(0f, 0f, 90f);
                    }
                }

            }

            //iWall -> this OR // iCorner -> this
            if (toLeft(x, y, 3) || toLeft(x, y, 4)) 
            {

                if (toTop(x, y, 3) || toTop(x, y, 4))
                {
                    return new Vector3(0f, 0f, 180f);
                }

                if(toRight(x, y, 3))
                {
                    Debug.Log("brug");
                }

                else
                {
                    return new Vector3(0f, 0f, 270f);
                }
            }

            //iWall -> this OR // iCorner -> this
            if (toRight(x, y, 3) || toRight(x, y, 4)) 
            {
                if(toTop(x, y, 3) || toTop(x, y, 4))
                {
                    return new Vector3(0f, 0f, 90f);
                }
                 
            }
        }

        //iWall
        if (value == 4) //iWall
        {

            //If between iWalls horizontally
            if ((toRight(x, y, 4) && toLeft(x, y, 4)))
            {
                    return new Vector3(0f, 0f, 90f);    
            }


            // iCorner -> this -> iWall
            if ((toRight(x, y, 3) && toLeft(x, y, 4)))
            {
                return new Vector3(0f, 0f, 90f);    
            }


            // iWall -> this -> iCorner
            if ((toRight(x, y, 4) && toLeft(x, y, 3)))
            {
                return new Vector3(0f, 0f, 90f); 
            }

            //iWall -> this -> nothing 
            if (toLeft(x, y, 4) && toBottom(x, y, 0))
            {
                return new Vector3(0f, 0f, 90f);
            }

            //nothing -> this -> iWall
            if (toRight(x, y, 4) && toBottom(x, y, 0))
            { 
                return new Vector3(0f, 0f, 90f);
            }

        }

        //tJunct
        if (value == 7) 
        {
            if(toLeft(x, y, 7) || toRight(x, y, 7))
            {
                if(toBottom(x, y, 4))
                {
                    return new Vector3(0f, 0f, 270f);
                }
                else
                {
                    return new Vector3(0f, 0f, 90f);
                }
                
            }
        }

        return new Vector3(0f, 0f, 0f);
    }


    bool toTop(int x, int y, int objCode)
    {
        if (x > 0)
        {
            if (levelMap[x - 1, y] == objCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool toBottom(int x, int y, int objCode)
    {
        if(x < columns-1)
        {
            if(levelMap[x + 1, y] == objCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool toLeft(int x, int y, int objCode)
    {
        if (y > 0)
        {
            if (levelMap[x, y - 1] == objCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool toRight(int x, int y, int objCode)
    {
        if (y < rows - 1)
        {
            if (levelMap[x, y + 1] == objCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}
