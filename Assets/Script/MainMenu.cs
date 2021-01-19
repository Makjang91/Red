using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    public GameObject settingsWindow;
    public GameObject selectModeWindow;
    public GameObject[] MainMenuButtons;


    public void Play()
    {
        SelectMode();
    }

    public void SelectMode()
    {
       selectModeWindow.SetActive(true);

        foreach (var e in MainMenuButtons)
        {
            e.SetActive(false);
        }

    }

    public void CloseSelectMode()
    {
       selectModeWindow.SetActive(false);

        foreach (var e in MainMenuButtons)
        {
            e.SetActive(true);
        }

    }

    public void Settings()
    {
        settingsWindow.SetActive(true);
    }

     public void CloseSettings()
    {
        settingsWindow.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
