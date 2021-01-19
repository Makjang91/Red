using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Transform cursor;
    Transform wHolder;

    Transform pistol;
    Transform lArm;
    Transform rArm;

    Transform left;
    Transform right;

    Transform head;

    Rigidbody2D rb;
    Transform red;

    Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        cursor = transform.Find("Cursor").transform;
        wHolder = transform.Find("weaponHolder").transform;

        pistol = wHolder.Find("pistol").transform;
        lArm = transform.Find("leftArmSolver/leftArmSolver_Target").transform;
        rArm = transform.Find("rightArmSolver/rightArmSolver_Target").transform;



        left = pistol.GetChild(0);
        right = pistol.GetChild(1);
        left.position = lArm.position;
        right.position = rArm.position;

        head = transform.Find("body/spine/upperBone/head") ;

        rb = GetComponent<Rigidbody2D>();
        red = GetComponentInParent<Transform>();

        firePoint = transform.Find("weaponHolder").Find("pistol").Find("firePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPuased)
            aimPistole();
    }

    public void aimPistole()
    {
        Vector3 direction = cursor.position - head.position;
        direction.Normalize();
        float angleDirection, angle;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (red.localScale.x == 1)
        {
            angleDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        else
        {
            angleDirection = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        }


        Vector3 localScale = Vector3.one;
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Red_HandAttack1"))
            return;
        if (Mathf.Abs(rb.velocity.x) > 0.001) // Red en train de bouger
        {
            if (angle >= 90 || angle < -80)
            {
                if (red.localScale.x == -1) // animaition des bras pour limiter l'angle
                {
                    wHolder.eulerAngles = new Vector3(0, 0, angleDirection);
                    lArm.position = left.position;
                    rArm.position = right.position;
                    head.eulerAngles = new Vector3(0, 0, angleDirection - 70);
                }
                else
                {
                    if(angle >= 0)
                    {
                        wHolder.eulerAngles = new Vector3(0, 0, 90);
                        lArm.position = left.position;
                        rArm.position = right.position;
                        head.eulerAngles = new Vector3(0, 0, 160);
                    }
                    else
                    {
                        wHolder.eulerAngles = new Vector3(0, 0, -70);
                        lArm.position = left.position;
                        rArm.position = right.position;
                        head.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (red.localScale.x == 1)
                {
                    wHolder.eulerAngles = new Vector3(0, 0, angleDirection);
                    lArm.position = left.position;
                    rArm.position = right.position;
                    head.eulerAngles = new Vector3(0, 0, angleDirection + 70);
                }
                else
                {
                    if (angle >= 0)
                    {
                        wHolder.eulerAngles = new Vector3(0, 0, -90);
                        lArm.position = left.position;
                        rArm.position = right.position;
                        head.eulerAngles = new Vector3(0, 0, -160);
                    }
                    else
                    {
                        wHolder.eulerAngles = new Vector3(0, 0, 70);
                        lArm.position = left.position;
                        rArm.position = right.position;
                        head.eulerAngles = new Vector3(0, 0, 0);
                    }
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
                red.localScale = localScale;
                firePoint.rotation = wHolder.rotation;
                head.eulerAngles = new Vector3(0, 0, angleDirection - 70);
            }
            else
            {
                localScale.x = 1f;
                red.localScale = localScale;
                head.eulerAngles = new Vector3(0, 0, angleDirection + 70);
            }
            
        }
    }

}
