
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class OpennedDoor : MonoBehaviour
{
   public GameObject player,tp;
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
            player.transform.position = new Vector2(tp.transform.position.x, tp.transform.position.y);

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
