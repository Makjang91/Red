using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    public bool estSur;
    public bool estSousPlayer;

    private GameObject touchedGameObject;

    // Start is called before the first frame update
    void Start()
    { 
        if (transform.name == "under")
            estSur = false;
        else if (transform.name == "up")
            estSur = true;
        else
            estSur = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("s") && estSousPlayer)
        {
            Physics2D.IgnoreCollision(transform.parent.GetComponent<PolygonCollider2D>(), touchedGameObject.GetComponent<CapsuleCollider2D>());
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<CapsuleCollider2D>() && collision.tag == "Player")
        {
            Physics2D.IgnoreCollision(transform.parent.GetComponent<PolygonCollider2D>(), collision.GetComponent<CapsuleCollider2D>(), !estSur);
            if(collision.tag == "Player")
                estSousPlayer = true;
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            touchedGameObject = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
            estSousPlayer = false;  
        touchedGameObject = null;
    }
}

