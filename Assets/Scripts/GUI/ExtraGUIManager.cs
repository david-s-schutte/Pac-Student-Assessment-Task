using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtraGUIManager : MonoBehaviour
{
    //Changes the scene depending on the scene name given
    public void onClickChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Closes the game
    public void onClickQuitGame()
    {
        Application.Quit();
    }
}
