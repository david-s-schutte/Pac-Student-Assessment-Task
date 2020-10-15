using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private static int rows; //number of rows
    private static int columns; //number of columns
    public GameObject mazeContainer; //container for the maze

    public GameObject empty;        //objCode: 0
    public GameObject oCorner;      //objCode: 1
    public GameObject oWall;        //objCode: 2
    public GameObject iCorner;      //objCode: 3
    public GameObject iWall;        //objCode: 4
    public GameObject pellet;       //objCode: 5
    public GameObject powerPellet;  //objCode: 6
    public GameObject tJunct;       //objCode: 7

    public static int[,] levelMap = {
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
        rows = levelMap.GetLength(0);
       // Debug.Log(rows);

        columns = levelMap.GetLength(1);
       // Debug.Log(columns);

        for (int r = 0; r < rows; r++) //i.e. y postion
        {
            for (int c = 0; c < columns; c++) //i.e. x position
            {
                SpawnObject(c, r, levelMap[r, c]);
            }
        }

    }

    private void SpawnObject(int x, int y, int objCode)
    {
        Vector3 spawnPosition = new Vector3(x * 1, y * 1);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = determineRotation(y, x, objCode);

        switch (objCode)
        {
            case 0:
                var new_empty = Instantiate(empty, spawnPosition, rotation, mazeContainer.transform);
                new_empty.name = ("x: " + x + ", y: " + y);
                break;
            case 1: 
                var new_oCorner = Instantiate(oCorner, spawnPosition, rotation, mazeContainer.transform); 
                new_oCorner.name = ("x: " + x + ", y: " + y); 
                break;
            case 2: 
                var new_oWall = Instantiate(oWall, spawnPosition, rotation, mazeContainer.transform); 
                new_oWall.name = ("x: " + x + ", y: " + y); 
                break;
            case 3: 
                var new_iCorner = Instantiate(iCorner, spawnPosition, rotation, mazeContainer.transform); 
                new_iCorner.name = ("x: " + x + ", y: " + y); 
                break;
            case 4: 
                var new_iWall = Instantiate(iWall, spawnPosition, rotation, mazeContainer.transform); 
                new_iWall.name = ("x: " + x + ", y: " + y); 
                break;
            case 5: 
                var new_pellet = Instantiate(pellet, spawnPosition, rotation, mazeContainer.transform); 
                new_pellet.name = ("x: " + x + ", y: " + y);
                break;
            case 6: 
                var new_powerPellet = Instantiate(powerPellet, spawnPosition, rotation, mazeContainer.transform); 
                new_powerPellet.name = ("x: " + x + ", y: " + y); 
                break;
            case 7: 
                var new_tJunct = Instantiate(tJunct, spawnPosition, rotation, mazeContainer.transform); 
                new_tJunct.name = ("x: " + x + ", y: " + y); 
                break;
        }
    }

    Vector3 determineRotation(int y, int x, int objCode)
    {
        //oCorner
        if (objCode == 1)
        {
            if (hasObjBelow(y, x, 2) && hasObjRight(y, x, 2))
            {
                return new Vector3(0f, 0f, 0f);
            }

            if (hasObjBelow(y, x, 2) && hasObjLeft(y, x, 2))
            {
                return new Vector3(0f, 0f, 270f);
            }

            if (hasObjLeft(y, x, 2) && hasObjAbove(y, x, 2))
            {
                return new Vector3(0f, 0f, 180f);
            }

            if (hasObjRight(y, x, 2) && hasObjAbove(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }


        }

        //oWall
        if (objCode == 2)
        {
            if (hasObjLeft(y, x, 2) && hasObjRight(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 2) && hasObjRight(y, x, 1))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 1) && hasObjRight(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 2) && hasObjRight(y, x, 7))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 7) && hasObjRight(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjRight(y, x, 2))
            {
                return new Vector3(0f, 0f, 90f);
            }

            return new Vector3(0f, 0f, 0f);
        }

        //iCorner
        if (objCode == 3)
        {
            //Top Right Corners
            if (hasObjRight(y, x, 4) && hasObjBelow(y, x, 4))
            {
                if (hasObjBelow(y - 1, x, 5) && hasObjRight(y, x + 2, 3))
                {
                    return new Vector3(0f, 0f, 90f);
                }

                if (hasObjRight(y, x + 1, 5))
                {
                    return new Vector3(0f, 0f, 270f);
                }
                return new Vector3(0f, 0f, 0f);
            }

            if (hasObjRight(y, x, 4) && hasObjBelow(y, x, 3))
            {

                if (hasObjAbove(y, x, 4) && hasObjRight(y, x + 1, 5))
                {
                    return new Vector3(0f, 0f, 180f);
                }

                if (hasObjAbove(y, x, 4) && hasObjRight(y, x + 1, 4))
                {
                    return new Vector3(0f, 0f, 90f);
                }

                return new Vector3(0f, 0f, 0f);
            }

            if (hasObjRight(y, x, 3) && hasObjBelow(y, x, 4))
            {
                if (hasObjBelow(y - 1, x, 5))
                {
                    return new Vector3(0f, 0f, 180f);
                }

                if (hasObjAbove(y + 1, x, 5))
                {
                    return new Vector3(0f, 0f, 270f);
                }

                return new Vector3(0f, 0f, 0f);
            }

            if (hasObjRight(y, x, 3) && hasObjBelow(y, x, 3))
            {
                return new Vector3(0f, 0f, 0f);
            }


            //Top Left Corners
            if (hasObjLeft(y, x, 4) && hasObjBelow(y, x, 4))
            {
                return new Vector3(0f, 0f, 270f);
            }

            if (hasObjLeft(y, x, 4) && hasObjBelow(y, x, 3))
            {
                return new Vector3(0f, 0f, 270f);
            }

            if (hasObjLeft(y, x, 3) && hasObjBelow(y, x, 4))
            {

                return new Vector3(0f, 0f, 270f);
            }

            //Bottom Left Corners
            if (hasObjAbove(y, x, 4) && hasObjRight(y, x, 4))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjAbove(y, x, 3) && hasObjRight(y, x, 4))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjAbove(y, x, 4) && hasObjRight(y, x, 3))
            {
                return new Vector3(0f, 0f, 90f);
            }

            //Bottom Right Corners
            if (hasObjAbove(y, x, 4) && hasObjLeft(y, x, 4))
            {
                return new Vector3(0f, 0f, 180f);
            }

            if (hasObjAbove(y, x, 3) && hasObjLeft(y, x, 4))
            {
                return new Vector3(0f, 0f, 180f);
            }

            if (hasObjAbove(y, x, 4) && hasObjLeft(y, x, 3))
            {
                return new Vector3(0f, 0f, 180f);
            }

            return new Vector3(0f, 0f, 0f);
        }

        //iWall
        if (objCode == 4) //iWall
        {
            if (hasObjLeft(y, x, 3) && hasObjRight(y, x, 3))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 3) && hasObjRight(y, x, 4))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 4) && hasObjRight(y, x, 3))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 4) && hasObjRight(y, x, 4))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjLeft(y, x, 4) && hasObjAbove(y, x, 0))
            {
                return new Vector3(0f, 0f, 90f);
            }

            if (hasObjAbove(y, x, 0) && hasObjRight(y, x, 4))
            {
                return new Vector3(0f, 0f, 90f);
            }

            return new Vector3(0f, 0f, 0f);
        }

        //tJunct
        if (objCode == 7)
        {
            if (hasObjLeft(y, x, 2) && hasObjRight(y, x, 7))
            {
                if (hasObjBelow(y, x, 4))
                {
                    return new Vector3(0f, 0f, 270f);
                }
                else
                {
                    return new Vector3(0f, 0f, 90f);
                }
            }

            if (hasObjLeft(y, x, 7) && hasObjRight(y, x, 2))
            {
                if (hasObjBelow(y, x, 4))
                {
                    return new Vector3(0f, 0f, 270f);
                }
                else
                {
                    return new Vector3(0f, 0f, 90f);
                }
            }

            return new Vector3(0f, 0f, 0f);
        }

        //If the objCode doesn't match any of the requirements
        return new Vector3(0f, 0f, 0f);
    }

    //If this object is above the object with objCode
    bool hasObjAbove(int y, int x, int objCode)
    {
        if (y < rows - 1)
        {
            if (levelMap[y + 1, x] == objCode)
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

    //If this object is below the object with objCode
    bool hasObjBelow(int y, int x, int objCode)
    {
        if (y > 0)
        {
            if (levelMap[y - 1, x] == objCode)
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

    //If this object has objCode on its left
    bool hasObjLeft(int y, int x, int objCode)
    {
        if (x > 0)
        {
            if (levelMap[y, x - 1] == objCode)
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

    //if this object has objCode on its right
    bool hasObjRight(int y, int x, int objCode)
    {
        if (x < columns - 1)
        {
            if (levelMap[y, x + 1] == objCode)
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

    //Returns what object is stored in the sent coordinates 
    public static int getCoordinates(int x, int y) {
        return levelMap[y, x];
    }

}
