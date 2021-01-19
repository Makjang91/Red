using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Loadlevel : MonoBehaviour
{
   
     public bool isRange;
     public Text interactUI;

    void Awake()
    {   
        interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
    }

    void Update()
    {
            if(isRange && Input.GetKeyDown(KeyCode.Space))
        {
             SceneManager.LoadScene("Stage2");

        }
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
    
        
        if(collision.CompareTag("Player"))
        {
            interactUI.enabled = true;
            isRange = true;
            
        }
      
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
         if(collision.CompareTag("Player"))
        {
            interactUI.enabled = false;
            isRange = false;

        }
    }
    
}
