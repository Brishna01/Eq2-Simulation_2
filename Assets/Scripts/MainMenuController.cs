using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup optionPanel;

    public void Jouer()
    {
        SceneManager.LoadScene("Scene Principale", LoadSceneMode.Single);
    }

    public void Quitter() 
    {
        Application.Quit();
    }
}
