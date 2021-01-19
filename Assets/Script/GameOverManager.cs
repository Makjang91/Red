using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    GameObject gameOverUI;
    /*
    public static GameOverManager instance;
    
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scéne");
            return;
        }

        instance = this;
    }*/

    public void Start()
    {
        gameOverUI = transform.Find("GameOverMenu").gameObject;
    }

    public void OnPlayerDeath()
    {
        gameOverUI.SetActive(true);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverUI.SetActive(false);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("StartManu");
    }
}
