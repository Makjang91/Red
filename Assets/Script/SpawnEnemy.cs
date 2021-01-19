using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public GameObject porte;

    Vector2 WhereToSpawn;
    
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            WhereToSpawn = new Vector2(porte.transform.position.x, porte.transform.position.y);
            Instantiate(enemy,WhereToSpawn,Quaternion.identity);
        }
    }
}
