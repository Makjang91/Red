using UnityEngine.SceneManagement;
using UnityEngine;

public class Exit_Room1 : MonoBehaviour
{
      public bool isRange;
    

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            isRange = true;
        }
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isRange == true)
        {
        if(collision.CompareTag("Player"))
        {
            
            SceneManager.LoadScene("Stage2");
            
        }
        }
    }
    
}
