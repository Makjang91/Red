using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float verticalMove;
    private Vector2 velocity = Vector2.zero;

    float movementSpeed = 10f;
    float runSpeed = 20f;
    float jumpVelocity = 30f;
    public bool estSurSol;
    bool run;

    public int layerPresent;
    public Transform cursor;

    private bool trig;
    private Collider2D objInteraction;

    private PlayerMovement movement;
    private PlayerCombat combat;
    int selectedWeapon;
    bool pause;
    float pauseTemps;
    float delaiPrisDegatTemps = 1;

    public Transform attackPoint;

    int handAttackDamge = 20;
    float handAttackRate = 2f; // pour controler entre temps des attaques
    float nextHandAttackTime = 0f;

    int pistolAttackDamge = 50;
    float pistolAttackRate = 1f; // pour controler entre temps des attaques
    float nextPistolAttackTime = 0f;

    bool takeCover;
    bool hide;

    LayerMask enemiesLayer;

    bool rewind;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cursor = GameObject.Find("Cursor").transform;
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        pauseTemps = 0;
        combat.SetDelaiPrisDegatTemps(delaiPrisDegatTemps);
        combat.SetHandDamage(handAttackDamge);
        combat.SetPistolDamage(pistolAttackDamge);
        takeCover = false;
        run = false;

        enemiesLayer = LayerMask.GetMask("Enemies");
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (!rewind)
        {

            if (Time.timeScale == 1f)
            {
                if (!hide)
                {
                    //Debug.Log( animator.GetCurrentAnimatorStateInfo(0).speed);
                    Movement();
                    Combat();
                }

                if (trig)
                {
                    InteractionObject();
                }

                if (estSurSol)
                {
                    //animator.SetBool("isJumping", false);
                }

                Sneak();
            }
        }
        
    }
    void Movement()
    {
        
        if (estSurSol)
        {

            Vector3 localScale = Vector3.one;
            
            if (Input.GetKey("s") && takeCover)
            {
                movement.Crouch();
            }
            
            if(Input.GetKey("d"))
            {   // bouger à droit
                if(run)
                    movement.MoveRight(runSpeed);
                else
                    movement.MoveRight(movementSpeed);

            }
            else if (Input.GetKey("q"))
            {   // bouger à gauche
                if(run)
                    movement.MoveLeft(runSpeed);
                else
                    movement.MoveLeft(movementSpeed);

            }
            else
            {   // on s'arrete quand il n'y a aucune touche appuyée
                movement.Stay();
            }

            if (Input.GetKeyDown("z") && estSurSol)
            {   // jump
                movement.Jump(jumpVelocity);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && estSurSol)
            {
                RunToggle();
            }

            // pour mettre l'animation quand Red s'arrete
            animator.SetFloat("direction", Mathf.Abs(rb.velocity.x));
        }

    }

    void RunToggle()
    {
        if (run == false)
            run = true;
        else
            run = false;
    }

    void Sneak()
    {
        if(run == true && rb.velocity.x > movementSpeed)
        {

            RaycastHit2D[] tabRc = Physics2D.CircleCastAll(transform.position, 5f, Vector2.one, 5f, enemiesLayer);

            foreach (RaycastHit2D rc in tabRc)
            {
                if (!rc.transform.GetComponent<Enemy>().GetVuPlayer())
                    rc.transform.GetComponent<Enemy>().LocaliserRed(transform.position);
            }
        }
    }

    void Combat()
    {
        selectedWeapon = gameObject.GetComponentInChildren<WeaponSwiching>().selectedWeapon;
        pause = combat.GetPause();
        pauseTemps = combat.GetPauseTemps();
        if (!pause)
        {
            if (selectedWeapon == 0)
            {
                
                if (Time.time >= nextHandAttackTime)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        combat.HandAttack();
                        nextHandAttackTime = Time.time + 1f / handAttackRate;
                    }
                }
            }
            else if (selectedWeapon == 1)
            {
                
                if (Time.time >= nextPistolAttackTime)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        combat.PistolAttack();
                        nextPistolAttackTime = Time.time + 1f / pistolAttackRate;
                    }
                }
            }
        }
        else
        {
            if (Time.time >= pauseTemps)
                combat.SetPause(false);
        }
    }

    // pour pouvoir faire la mise à jour toutes les frames
    // au lieu d'utiliser des triggerEnter2D en triggerExit2D
    void InteractionObject()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (objInteraction.GetComponent<DoorScript>() != null) //verifie si c'est door
            {
                if (objInteraction.GetComponent<BoxCollider2D>().enabled)
                {
                    objInteraction.GetComponent<DoorScript>().Open();
                }
                else
                {
                    objInteraction.GetComponent<DoorScript>().Close();
                }
            }
            else if (objInteraction.GetComponent<ClosetScript>() != null)
            {
                ClosetScript cs = objInteraction.GetComponent<ClosetScript>();
                if (cs.EstDedans())
                {
                    cs.Leave();
                    hide = false;
                }
                else
                {
                    cs.Enter();
                    hide = true;
                }
            }
            
        }

        if (objInteraction.tag == "Box")
        {
            if (animator.GetBool("crouching"))
                takeCover = false;
            else
                takeCover = true;
        }
    }

    public bool estSeCache() { return hide; }
    public void setRewind(bool rw) { rewind = rw; }
    public bool getRewind() { return rewind; }

    // quand le player touche
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        { // sol
            layerPresent = 11;
            estSurSol = true;
            animator.SetBool("isJumping", false);
        }
        else
        {
            //layerPresent = collision.gameObject.layer;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 10)
        {
            estSurSol = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 11)
            estSurSol = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            trig = true;
            objInteraction = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            trig = false;
            objInteraction = null;
            takeCover = false;
        }
        
    }

}
