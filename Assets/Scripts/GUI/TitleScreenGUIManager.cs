using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenGUIManager : MonoBehaviour
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
