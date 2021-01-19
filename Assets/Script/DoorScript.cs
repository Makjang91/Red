using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    bool estOuvert = false;
    public void Open()
    {
        GetComponent<Animator>().SetBool("door", true);
        GetComponent<BoxCollider2D>().enabled = false;
        estOuvert = true;
        gameObject.tag = "Untagged";
    }

    public void Close()
    {
        GetComponent<Animator>().SetBool("door", false);
        GetComponent<BoxCollider2D>().enabled = true;
        estOuvert = false;
        gameObject.tag = "Impassable";
    }

    public bool EstOuvert() { return estOuvert; }

}
