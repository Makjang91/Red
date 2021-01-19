using UnityEngine;
public class TrapDamage : MonoBehaviour
{
    public int trapDamage = 20;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            //playerHealth.TakeDamage(trapDamage);
        }
    }
}
