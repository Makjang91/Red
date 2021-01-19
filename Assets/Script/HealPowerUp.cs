using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerCombat.instance.HealPlayer(50);
        }
        
    }
}
