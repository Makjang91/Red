using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMode : MonoBehaviour
{
    public string levelToLoad;
    public string levelToLoad2;
    
    public void Story()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Survival()
    {
        SceneManager.LoadScene(levelToLoad2);
    }
}
