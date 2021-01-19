using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackAim : MonoBehaviour
{

    Transform cible;
    Transform tete;
    Rigidbody2D rb;
    Transform wHolder;
    Transform pistol;
    Transform lArm;
    Transform rArm;
    Transform left;
    Transform right;

    // Start is called before the first frame update
    void Start()
    {
        cible = transform.Find("target");
        tete = transform.Find("body/spine/upperBone/head");
        rb = GetComponent<Rigidbody2D>();

        wHolder = transform.Find("weaponHolder");

        pistol = wHolder.Find("pistol");
        lArm = transform.Find("leftArmSolver/leftArmSolver_Target");
        rArm = transform.Find("rightArmSolver/rightArmSolver_Target");


        left = pistol.GetChild(0);
        right = pistol.GetChild(1);

        left.position = lArm.position;
        right.position = rArm.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = cible.position - tete.position;
        direction.Normalize();
        float angleDirection, angle;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (transform.localScale.x == 1)
        {
            angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        else
        {
            angleDirection = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        }

        Vector3 localScale = Vector3.one;

        if (Mathf.Abs(rb.velocity.x) > 0.001) // Red en train de bouger
        {

            if (angle >= 90 || angle < -90)
            {
                if (transform.localScale.x == -1) // animaition des bras pour limiter l'angle
                {
                    wHolder.eulerAngles = new Vector3(0, 0, angleDirection);
                    lArm.position = left.position;
                    rArm.position = right.position;
                    tete.eulerAngles = new Vector3(0, 0, angleDirection - 70);
                }
            }
            else
            {
                if (transform.localScale.x == 1)
                {
                    wHolder.eulerAngles = new Vector3(0, 0, angleDirection);
                    lArm.position = left.position;
                    rArm.position = right.position;
                    tete.eulerAngles = new Vector3(0, 0, angleDirection + 70);
                }
            }
        }
        else // Quand Red ne bouge pas
        {
            wHolder.eulerAngles = new Vector3(0, 0, angleDirection);
            lArm.position = left.position;
            rArm.position = right.position;
            if (angle >= 90 || angle < -90)
            {
                localScale.x = -1f;
                transform.localScale = localScale;
                pistol.Find("firePoint").rotation = wHolder.rotation;
                tete.eulerAngles = new Vector3(0, 0, angleDirection - 70);
            }
            else
            {
                localScale.x = 1f;
                transform.localScale = localScale;
                tete.eulerAngles = new Vector3(0, 0, angleDirection + 70);
            }

        }
    }
}
