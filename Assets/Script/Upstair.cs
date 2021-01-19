using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upstair : MonoBehaviour
{

    private GameObject player;
    public bool estSousPlayer;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s") && estSousPlayer)
        {
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            estSousPlayer = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            transform.parent.GetComponent<BoxCollider2D>().enabled = true;
            estSousPlayer = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            estSousPlayer = false;
        }
    }

}
