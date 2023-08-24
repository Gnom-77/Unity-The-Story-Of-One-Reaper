using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("FirstLocation");
    }

    public void GameSettings()
    {
        Debug.Log("Game Settings is Open");
    }

    public void ExitGame()
    {
        Debug.Log("Game was Close");
    }
}
