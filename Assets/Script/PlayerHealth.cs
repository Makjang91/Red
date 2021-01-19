using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public HealthBar healthbar;
    public bool isInvicible = false;
    public SpriteRenderer graphics;
    public float InvincibilityFlashDelay = 0.2f;
    public float InvincibilityTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

   /* // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }
    }

    // Le joueur prend des degats et actualise la barre de vie
    public void TakeDamage(int damage)
    {
        if(!isInvicible)
        {
            currentHealth -= damage;
            healthbar.SetHealth(currentHealth);
            isInvicible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvincibilityDelay());
        }
    }

    // Le joueur clignote tant qu'il est invincible
    public IEnumerator InvincibilityFlash()
    {
        while (isInvicible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(InvincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(InvincibilityFlashDelay);
        }
    }

    // L'invincibilité du joueur ne dure que 
    public IEnumerator HandleInvincibilityDelay()
    {
        yield return new WaitForSeconds(InvincibilityTime);
        isInvicible = false;
    }*/
}
