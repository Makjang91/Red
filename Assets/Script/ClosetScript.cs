using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetScript : MonoBehaviour
{
    bool estDedans = false;
    GameObject player;
    WeaponSwiching ws;
    bool rewind;
    private void Start()
    { 
        player = GameObject.Find("Red");
        ws = player.transform.Find("weaponHolder").GetComponent<WeaponSwiching>();
    }
    public void Enter()
    {
        ws.SelectWeapon(0);        

        GetComponent<Animator>().SetBool("interact",true);
        estDedans = true;
        player.transform.position = transform.position + new Vector3(-1,-1);

        StartCoroutine(EnterIEnum(0.5f));

    }

    IEnumerator EnterIEnum(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.GetComponent<Animator>().SetTrigger("ClosetEntering");
        StartCoroutine(EndOfAnimation(1f));
        GetComponent<Animator>().SetBool("interact", false);
    }


    public void Leave()
    {
        GetComponent<Animator>().SetBool("interact",true);
        estDedans = false;
        StartCoroutine(LeaveIEnum(0.5f));

    }

    IEnumerator LeaveIEnum(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        player.transform.position += new Vector3(0.01f, 0.01f); // pour évier la collision avec le sol
        player.GetComponent<Animator>().SetTrigger("ClosetLeaving");
        GetComponent<SpriteRenderer>().sortingOrder = 4;
        ws.SelectWeapon(ws.selectedWeapon);
        GetComponent<Animator>().SetBool("interact", false);
    }

    IEnumerator EndOfAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<SpriteRenderer>().sortingOrder = 6;


    }

    public bool EstDedans() { return estDedans; }
    public bool getRewind() { return rewind; }
    public void setRewind(bool rw) { rewind = rw;}
}
