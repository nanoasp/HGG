using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyResource;

    public float cooldown;
    float timer;
    public float enemySpeed;
    public int enemyHp;
    public Vector2 enemyVelocity;
    public int numberOfEnemies;
    public bool enemyJump;
    public float enemyJumpVelocity;
    public bool enemySpin;
    public float enemySpinVelocity;

    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfEnemies > 0)
        {
            if (timer <= 0)
            {
                SpawnEnemies();
                --numberOfEnemies;
                timer = cooldown;
            }
            timer -= Time.deltaTime;
        }
    }

    public GameObject SpawnEnemies()
    {
        // Spawn Enemies
        GameObject spawnedEnemies = new GameObject();

        spawnedEnemies = Instantiate(enemyResource, transform);

        var b = spawnedEnemies.GetComponent<BasicAi>();
        b.speed = enemySpeed;
        b.velocity = enemyVelocity;
        b.startHp = enemyHp;
        b.jump = enemyJump;
        b.jumpVelocity = enemyJumpVelocity;
        b.spin = enemySpin;
        b.spinVelocity = enemySpinVelocity;
        
        return spawnedEnemies;
    }
}