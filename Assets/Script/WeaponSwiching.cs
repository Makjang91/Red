using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwiching : MonoBehaviour
{
    public int selectedWeapon = 0;

    Animator animator;
    List<string> AnimControllers = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.transform.parent.GetComponent<Animator>();

        // ajout des paths de RuntimeAnimatorController
        int i = 0;
        foreach (Transform weapon in transform)
        {
            switch(weapon.name)
            {
                case "hand":
                    AnimControllers.Add("animation/Animator/RedHand");
                    break;
                case "pistol":
                    AnimControllers.Add("animation/Animator/RedPistol");
                    break;
            }
            i++;
        }

        SelectWeapon();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        int previousSelectWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (selectedWeapon != previousSelectWeapon)
            SelectWeapon();
        */
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform )
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimControllers[i]);
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }


    /*
     * Précondition : i doit être entre 0 et 1 (ça peut augmenter si on ajoute plus d'arme)
     */
    public void SelectWeapon(int ind)
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == ind)
            {
                weapon.gameObject.SetActive(true);
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(AnimControllers[i]);
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
