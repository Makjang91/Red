using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    enum weapon { Hand, Pistol};

    public Animator animator;


    public int maxHealth = 100;
    public int currentHealth;

    string holdingWeapon;

    // mouvement
    Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    float vitesseMarche = 5f;
    float vitesseCourse;
    float distance = 2f;
    bool marcheGauche;
    bool trouvePlayer;
    Transform groundDetector;

    bool pause;
    float pauseTemps;
    float delaiPrisDegatTemps = 0.5f;

    float prchHandAttackTemps = 0f;
    float handAttackRate = 0.5f;
    Transform handAttackPoint;
    float handAttackRange = 1f;
    int handAttackDamge = 20;
    public LayerMask redLayers;
    LayerMask enemiesLayer;

    float tempsRecherche = 10f;
    Vector2 playerPosition;
    bool vuPlayer;
    float observedTime;
    
    float suspicionTime = 0f;
    float recognizingTime = 2f;
    bool suspiction;
    bool aDebutEscalier;

    private Collider2D objInteraction;
    GameObject bulletPrefab;
    Transform firePoint;
    ParticleSystem muzzleflash;

    public AudioSource audioSource;
    public AudioClip sound;
    public AudioClip soundShot;

    bool rewind;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        groundDetector = transform.Find("groundDetector");
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(9, 9);

        marcheGauche = true;
        trouvePlayer = false;
        aDebutEscalier = false;
        pause = false;

        holdingWeapon = transform.Find("weaponHolder").GetChild(0).name;
        handAttackPoint = transform.Find("weaponHolder/hand/attackPoint");
        redLayers = LayerMask.GetMask("Player");
        enemiesLayer = LayerMask.GetMask("Enemies");

        vuPlayer = false;

        suspiction = false;

        bulletPrefab = Resources.Load<GameObject>("gameObjects/Bullet");
        firePoint = transform.Find("weaponHolder/pistol/firePoint");
        muzzleflash = GetComponentInChildren<ParticleSystem>();

        if (holdingWeapon == "hand")
            vitesseCourse = 20f;
        else
            vitesseCourse = 10f;

        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rewind)
        {

            if (!pause)
            {
                if (!trouvePlayer)
                {

                    if (suspiction)
                    {
                        Deplacement(0);
                    }
                    else
                    {
                        if (!vuPlayer)
                            Petrole();
                        else
                            Recherche();
                    }
                    Detection();
                }
                else
                {
                    MetPlayer();
                }



            }
            else
            {
                if (Time.time >= pauseTemps)
                    pause = false;
            }
        }
        else
        {
            vuPlayer = false;
            suspiction = false;
            aDebutEscalier = false;
            trouvePlayer = false;
            if (holdingWeapon == "hand")
                GetComponent<Animator>().SetBool("punch", false);
            else if (holdingWeapon == "pistol")
                GetComponent<Animator>().SetBool("shoot", false);
        }
    }


    /*
     * Quand l'ennemi trouve pas de Player, il se balade
     */
    void Petrole()
    {
        
        if (marcheGauche)
        {
            Deplacement(-vitesseMarche);
            
        }
        else
        {
            Deplacement(vitesseMarche);
        }
        


        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, distance);
        Collider2D circleInfo = Physics2D.OverlapCircle(groundDetector.position, 0.5f);

        Vector3 localScale = Vector3.one;
        bool turn = false;
        if (groundInfo.collider == false)
            turn = true;
        if (circleInfo != false)
        {
            string nom = circleInfo.tag;
            if (nom == "Impassable" || nom == "IgnoreObjects")
                turn = true;
        }
        if (turn)
        {
            if (marcheGauche == true)
            {
                marcheGauche = false;
            }
            else
            {
                marcheGauche = true;
            }

        }

        
        
        
    }

    /*
     * Fonction permet d'ignorer les gameObject pour qu'ils n'empechent pas de mouvement de l'ennemi
     */
    void IgnoreObjects(GameObject[] objets)
    {
        foreach (GameObject obj in objets)
        {
            if(obj.layer == 10)
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), obj.GetComponent<PolygonCollider2D>());
        }
    }

    /*
     * Cette fonction se fait appeler par les autres gameobjects
     */
    public void TakeDemage(int damage)
    {
        if (!pause)
        {
            if (trouvePlayer)
            {
                currentHealth -= damage;
            }
            else // attaquer par derriere
            {
                currentHealth = 0;
            }
            // hurt animation
            

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                rb.velocity = Vector2.zero;
                pauseTemps = Time.time + delaiPrisDegatTemps;
                pause = true;
            }


            // reconnaitre après avoir été attaqué
            trouvePlayer = true;
            playerPosition = GameObject.Find("Red").transform.position;
            vuPlayer = true;
            observedTime = Time.time;
            if (playerPosition.x - transform.position.x > 0)
            {
                marcheGauche = false;
            }
            else
            {
                marcheGauche = true;
            }

            animator.SetBool("hurt",true);

            StartCoroutine(waitTime(0.3f));
        }
    }

    
   
    void Die()
    {
        // die animation
        animator.SetBool("isDead",true);

        // disable the enemy
        GetComponent<Collider2D>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        this.enabled = false;

        transform.Find("weaponHolder").gameObject.SetActive(false);

        dead = true;
    }

    public void rewindDie()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        this.enabled = true;

        transform.Find("weaponHolder").gameObject.SetActive(true);

        dead = false;
    }

    /*
     * Fonction qui est lancée quand l'ennemi a trouvé Red (sous ces yeux)
     */
    void MetPlayer()
    {
        if (holdingWeapon == "hand")
            HandAttack();
        else if (holdingWeapon == "pistol")
            PistolAttack();

    }

    /*
     * Fonction qui fait deplacer l'ennemi jusuqu'à l'endroit qui peut attaquer le Player
     */
    void HandAttack()
    {

        float disPlayer = Mathf.Abs(transform.position.x - GameObject.Find("Red").transform.position.x);

        if (disPlayer < 3f)
        {
            Deplacement(0);
        }

        // direction d'attack
        Vector3 localScale = Vector3.one;
        if (transform.position.x >= GameObject.Find("Red").transform.position.x)
        {
            localScale.x = -1f;
            transform.localScale = localScale;
        }
        else
        {
            localScale.x = 1f;
            transform.localScale = localScale;
        }

        

         if (Time.time >= prchHandAttackTemps)
        {
            if (disPlayer < 3f)
            {
                Punch();
                prchHandAttackTemps = Time.time + 1f / handAttackRate;
            }
            else
            {
                if (transform.position.x - GameObject.Find("Red").transform.position.x > 0) // red est à gauche
                    Deplacement(-vitesseCourse);
                else
                    Deplacement(vitesseCourse);
            }
        }
    }


    IEnumerator PunchIEnum(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Collider2D[] hitRed = Physics2D.OverlapCircleAll(handAttackPoint.position, handAttackRange, redLayers);
        foreach (Collider2D rd in hitRed)
        {
            rd.GetComponent<PlayerCombat>().TakeDemage(handAttackDamge);
        }

        animator.SetBool("punch", false);
    }
    void Punch()
    {
        animator.SetBool("punch",true);
        StartCoroutine(PunchIEnum(0.5f));
        
    }

    void PistolAttack()
    {
        float disPlayer = Mathf.Abs(transform.position.x - GameObject.Find("Red").transform.position.x);

        if (disPlayer < 30f)
        {
            Deplacement(0);
        }

        // direction d'attack
        Vector3 localScale = Vector3.one;
        if (transform.position.x >= GameObject.Find("Red").transform.position.x)
        {
            localScale.x = -1f;
            transform.localScale = localScale;
        }
        else
        {
            localScale.x = 1f;
            transform.localScale = localScale;
        }

        if (Time.time >= prchHandAttackTemps)
        {
            if (disPlayer < 30f)
            {
                Tirer();
                prchHandAttackTemps = Time.time + 1f / handAttackRate;
            }
            else
            {
                if (transform.position.x - GameObject.Find("Red").transform.position.x > 0) // red est à gauche
                    Deplacement(-vitesseCourse);
                else
                    Deplacement(vitesseCourse);
            }
        }
    }

    void Tirer()
    {
        audioSource.PlayOneShot(soundShot);
        Vector2 direction;
        
        direction = GameObject.Find("Red").transform.position - transform.Find("body/spine/upperBone/head").position + new Vector3(0,1.5f,0);
        direction.Normalize();


        Vector3 localScale = Vector3.one;
        GameObject go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        go.GetComponent<Bullet>().setCibleLayer(8);
        if (transform.localScale.x == -1)
        {
            localScale.x = -1f;
            go.transform.localScale = localScale;
        }
        go.GetComponent<Bullet>().direction = direction;
        //animation
        muzzleflash.Play();

        animator.SetBool("shoot",true);

        StartCoroutine(waitTime(0.2f));
    }

    /*
     * alerter son autour où red se situe
     */
    void Alerter()
    {
        Collider2D[] tabCol = Physics2D.OverlapCircleAll(transform.position, 15f, enemiesLayer) ;
        foreach(Collider2D col in tabCol)
        {
            col.transform.GetComponent<Enemy>().LocaliserRed(playerPosition);
        }

    }

    public void LocaliserRed(Vector2 pos)
    {
        playerPosition = pos;
        vuPlayer = true;
        observedTime = Time.time;
    }

    /*
     * la fonction détecte ceux qui sont devant l'ennemi
     * s'il trouve Player à travers mur ou objets (impassable)
     * il ne voit pas, sinon il change les booleans.
     */
    void Detection()
    {
        if (GameObject.Find("Red").GetComponent<Player>().estSeCache())
        {
            return;
        }

        RaycastHit2D[] checkPlayer;

        if (marcheGauche)
        {
            checkPlayer = Physics2D.RaycastAll(transform.position, Vector2.left, 100);
        }
        else
        {
            checkPlayer = Physics2D.RaycastAll(transform.position, Vector2.right, 100);
        }

        foreach (RaycastHit2D rc in checkPlayer)
        {
            if (rc.transform.tag == "Impassable")
            {
                suspiction = false;
                trouvePlayer = false;
                break;
            }
            else if (rc.transform.tag == "Player")
            {
                if (suspiction)
                {
                    
                    if (Time.time >= suspicionTime + recognizingTime)
                    {
                        playerPosition = rc.transform.position;
                        vuPlayer = true;
                        observedTime = Time.time;
                        trouvePlayer = true;
                        Alerter();
                        animator.SetBool("Suspicious", false);
                    }
                }
                else
                {
                    suspiction = true;
                    suspicionTime = Time.time;
                    audioSource.PlayOneShot(sound);
                    // point d'exclamation
                    animator.SetBool("Suspicious", true);
                }
                break;
            }
        }
    }

    void Recherche()
    {
        Debug.Log("Recherche");
        if (Time.time >= observedTime + tempsRecherche)
        {
            vuPlayer = false;
        }
        else
        {
            bool deplacable = true;
            /*if ((transform.position.x < playerPosition.x - 1 || transform.position.x > playerPosition.x + 1) 
                && (transform.position.y < playerPosition.y - 1 || transform.position.y > playerPosition.x + 1))
            {    */            
                RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, distance);
                Collider2D circleInfo = Physics2D.OverlapCircle(groundDetector.position, 0.5f);
                
                if (groundInfo.collider == false)
                    deplacable = false;
                
                if (circleInfo != false)
                {
                    // ouvrir la porte
                    string tag = circleInfo.tag;
                    if(circleInfo.gameObject.layer == 12 && tag == "Impassable" && objInteraction)
                    {
                        if (objInteraction.GetComponent<BoxCollider2D>().enabled)
                        {
                            objInteraction.GetComponent<Animator>().SetBool("door", true);
                            objInteraction.GetComponent<BoxCollider2D>().enabled = false;
                            objInteraction.tag = "Untagged";
                        }
                        deplacable = true;
                    }

                }

                if (deplacable)
                {
                    
                    if (Mathf.Abs(playerPosition.y- transform.position.y) > 1)
                    {
                      ChercherEscalier();
                    }

                    else // l'ennemi et Red sont sur le meme étage
                    {
                        if (transform.position.x > playerPosition.x)
                        {
                            Deplacement(-vitesseCourse);
                        }
                        else
                        {
                            Deplacement(vitesseCourse);
                        }
                    }
                }  
            //} 
        }
    }

    void ChercherEscalier()
    {
        
        RaycastHit2D[] coteGauche;
        RaycastHit2D[] coteDroite;
        RaycastHit2D escalierGauche = new RaycastHit2D();
        RaycastHit2D escalierDroite = new RaycastHit2D();
        RaycastHit2D escalierPlusPres;

        if (playerPosition.y > transform.position.y + 10)
        {   // en haut
            coteGauche = Physics2D.RaycastAll(transform.position, Vector2.left, 200);
            coteDroite = Physics2D.RaycastAll(transform.position, Vector2.right, 200);
        }
        else
        {   // en bas
            coteGauche = Physics2D.RaycastAll(transform.Find("pathDetector").position, Vector2.left, 200);
            coteDroite = Physics2D.RaycastAll(transform.Find("pathDetector").position, Vector2.right, 200);
        }

        // chercher les escaliers les plus proches
        foreach (RaycastHit2D rc in coteGauche)
        {
            if (rc.transform.tag == "IgnoreObject")
            {
                escalierGauche = rc;
                break;
            }
        }
        foreach (RaycastHit2D rc in coteDroite)
        {
            if (rc.transform.tag == "IgnoreObject")
            {
                escalierDroite = rc;
                break;
            }
        }
        if (escalierGauche.transform != null)
        {
            if (escalierDroite.transform != null)
            {
                if (transform.position.x - escalierGauche.transform.position.x > escalierDroite.transform.position.x - transform.position.x)
                    escalierPlusPres = escalierDroite;
                else
                    escalierPlusPres = escalierGauche;
            }
            else
                escalierPlusPres = escalierGauche;
        }
        else
        {
            if (escalierDroite.transform != null)
                escalierPlusPres = escalierDroite;
            else // il y a pas d'escalier proche
            {
                if (playerPosition.y - transform.position.y > 0)
                {
                    /*vuPlayer = false;
                    trouvePlayer = false;
                    suspiction = false;*/
                    return;
                }
                else
                {
                    if (!aDebutEscalier)
                    {
                        /*vuPlayer = false;
                        trouvePlayer = false;
                        suspiction = false;*/
                        return;
                    }
                    else
                    {
                        Deplacement(vitesseCourse);
                        return;
                    }
                }
                
                
            }
        }
        
        if (playerPosition.y - transform.position.y < 0)
        {
            if (transform.position.x > escalierPlusPres.transform.Find("upstair").position.x + 2)
                Deplacement(-vitesseCourse);
            else if (transform.position.x < escalierPlusPres.transform.Find("upstair").position.x - 2)
                Deplacement(vitesseCourse);
            else // arriver à l'escalier
            {
                Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), escalierPlusPres.transform.GetComponent<BoxCollider2D>());
                Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), escalierPlusPres.transform.GetComponent<PolygonCollider2D>(), false);
            }
        }
        else
        {
            if (!aDebutEscalier)
            {
                if (transform.position.x > escalierPlusPres.transform.Find("debutEscalier").position.x)
                    Deplacement(-vitesseCourse);
                else if (transform.position.x < escalierPlusPres.transform.Find("debutEscalier").position.x - 6)
                    Deplacement(vitesseCourse);
                else // arriver à l'escalier
                {
                    Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), escalierPlusPres.transform.GetComponent<BoxCollider2D>(), true);
                    Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), escalierPlusPres.transform.GetComponent<PolygonCollider2D>(), false);
                    aDebutEscalier = true;
                }
            }
            else
            {
                if(transform.position.y < playerPosition.y + 2)
                {
                    Deplacement(vitesseCourse);
                    marcheGauche = false;
                }
            }
        }

    }
    

    void Deplacement(float v)
    { 
        rb.velocity = Vector2.SmoothDamp(rb.velocity, new Vector2(v, rb.velocity.y), ref velocity, 0.05f);
        animator.SetFloat("direction", Mathf.Abs(v));

        Vector3 localScale = Vector3.one;
        if (rb.velocity.x > 0)
        {
            if (transform.localScale.x != 1f)
            {
                localScale.x = 1f;
                transform.localScale = localScale;
            }
        }
        else
        {
            if (transform.localScale.x != -1f)
            {
                localScale.x = -1f;
                transform.localScale = localScale;
            }
        }
    }

    IEnumerator waitTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (holdingWeapon == "hand")
            GetComponent<Animator>().SetBool("punch", false);
        else if (holdingWeapon == "pistol")
            GetComponent<Animator>().SetBool("shoot", false);
        GetComponent<Animator>().SetBool("hurt", false);
    }

    public bool GetPause() { return pause; }

    public bool GetVuPlayer() { return vuPlayer; }

    public bool getRewind() { return rewind; }

    public void setRewind(bool rw) { rewind = rw; }
    public bool getDead() { return dead; }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            //estSurSol = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            //estSurSol = false;
        }
        else if (collision.gameObject.layer == 10)
        {
            Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), collision.transform.GetComponent<BoxCollider2D>(), false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
            objInteraction = collision;
        else if(collision.gameObject.layer == 10 && !vuPlayer)
        {
            if(collision.transform.parent.GetComponent<PolygonCollider2D>() != null)
                Physics2D.IgnoreCollision(transform.GetComponent<CapsuleCollider2D>(), collision.transform.parent.GetComponent<PolygonCollider2D>());
        }
    }

    private void OnTriggerExit2D()
    {
        objInteraction = null;
    }
}
