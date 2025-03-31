using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    public void Jouer()
    {
        SceneManager.LoadScene("Scene Principale", LoadSceneMode.Single);
    }

    public void Options()
    {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

    public void Retourner()
    {
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
    }

    public void Quitter() 
    {
        Application.Quit();
    }
}
