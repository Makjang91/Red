using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public Transform mousePosition;
    public Vector2 direction;

    int handAttackDamge;

    int pistolAttackDamge;

    int selectedWeapon;

    public Transform firePoint;
    public GameObject bulletPrefab;

    ParticleSystem muzzleflash;

    bool pause;
    float pauseTemps;
    float delaiPrisDegatTemps = 1;
    int maxHealth = 100;
    int currentHealth;

    public AudioSource audioSource;
    public AudioClip sound;

    public static PlayerCombat instance;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerCombat dans la scéne");
            return;
        }

        instance = this;
    }

    void Start()
    { 
        mousePosition = transform.Find("Cursor").transform;

        //pistole
        bulletPrefab = Resources.Load<GameObject>("gameObjects/Bullet");
        muzzleflash = GetComponentInChildren<ParticleSystem>();

        // health bar
        currentHealth = maxHealth;
        GameObject.Find("UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetMaxHealth(maxHealth);
    }
    
    public void HandAttack()
    {
        // direction d'attaque
        // normalise la position du souris
        direction = mousePosition.position - mousePosition.parent.position;

        direction.Normalize();


        attackPoint = transform.Find("weaponHolder").Find("hand").Find("attackPoint");


        // animation
        GetComponent<Animator>().SetTrigger("handAttack");
        
        // detecte les ennemies dans le range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Demage les ennemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDemage(handAttackDamge);
        }

    }

    public void PistolAttack()
    {
        direction = mousePosition.position - transform.Find("body/spine/upperBone/head").position;
        
        direction.Normalize();

        firePoint = transform.Find("weaponHolder").Find("pistol").Find("firePoint");

        Vector3 localScale = Vector3.one;
        GameObject go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        go.GetComponent<Bullet>().setDamage(pistolAttackDamge);
        go.GetComponent<Bullet>().setCibleLayer(9);
        if (transform.localScale.x == -1) {
            localScale.x = -1f;
            go.transform.localScale = localScale;
        }
        go.GetComponent<Bullet>().direction = direction;
        //animation
        GetComponent<Animator>().SetBool("shoot", true) ;
        muzzleflash.Play();
        audioSource.PlayOneShot(sound);


        StartCoroutine(waitTime(0.3f));
    }

    IEnumerator waitTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponent<Animator>().SetBool("shoot", false);
        GetComponent<Animator>().SetBool("hurt", false);
    }

    public void HealPlayer(int amount)
    {
        if(currentHealth + amount > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;

        GameObject.Find("UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth(currentHealth);
    }

    /*
     * précondition : pause est faux 
     */
    public void TakeDemage(int damage)
    {
        currentHealth -= damage;
        // hurt animation
        GetComponent<Animator>().SetBool("hurt",true);

        GameObject.Find("UI").transform.Find("HealthBar").GetComponent<HealthBar>().SetHealth(currentHealth);

        //verifie si le joueur est toujour vivant
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            pause = true;
            pauseTemps = Time.time + delaiPrisDegatTemps;
        }

        StartCoroutine(waitTime(0.3f));
    }

    public void Die()
    {

        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerMovement.instance.rb.velocity = Vector3.zero;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("weaponHolder/pistol").GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Animator>().SetBool("isDead", true);
        GameObject.Find("UI").GetComponent<GameOverManager>().OnPlayerDeath();
    }

    public bool GetPause() { return pause; }
    public void SetPause(bool p) { pause = p; }
    public float GetPauseTemps() { return pauseTemps;}
    public void SetDelaiPrisDegatTemps(float t) { delaiPrisDegatTemps = t; }
    public void SetHandDamage(int d) { handAttackDamge = d; }
    public void SetPistolDamage(int d) { pistolAttackDamge = d; }


    
}