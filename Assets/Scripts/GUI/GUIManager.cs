using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{ 
    public void onClickChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void onClickQuitGame() 
    {
        Application.Quit();
    }
}
