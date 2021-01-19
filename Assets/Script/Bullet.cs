using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3000f;
    public int damage;
    Rigidbody2D rb;
    public Vector2 direction;
    float creationTime;
    float existingTime = 20f;
    int cibleLayer;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * speed);
        creationTime = Time.time;
        active = true;

        GameObject.Find("UI").GetComponent<TimeManager>().addBullet(gameObject);

    }

    private void FixedUpdate()
    {
        if (Time.time > creationTime + existingTime)
        {
            GameObject.Find("UI").GetComponent<TimeManager>().destroyBullet(gameObject);
            Destroy(gameObject);
        }
    }

    public void setDamage(int d) { damage = d; }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (active)
        {
            if(cibleLayer == 9) // tiré par player
            {
                // detecte les ennemies dans le range
                Enemy enemy = hitInfo.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (!enemy.GetPause())
                    {
                        enemy.TakeDemage(damage);
                        destroyBullet();
                    }
                }
            }
            else if(cibleLayer == 8) // tiré par ennemi
            {
                PlayerCombat pc = hitInfo.GetComponent<PlayerCombat>();
                if (pc != null)
                {
                    if (!pc.GetPause())
                    {
                        pc.TakeDemage(10);
                        destroyBullet();
                    }
                }
            }
            if (hitInfo.gameObject.tag == "Impassable" || hitInfo.gameObject.layer == 13)
                destroyBullet();


            
        }
    }

    public void destroyBullet()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        rb.velocity = new Vector2(0, 0);
    }

    public void rewind()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        active = false;
    }

    public float getCreationTIme() { return creationTime; }

    public void setCibleLayer(int layer) { cibleLayer = layer; }

}
