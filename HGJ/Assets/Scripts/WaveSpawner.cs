using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] enemyResource;

    void Start()
    {
        Invoke("SpawnWave1", 2.0f);
        Invoke("SpawnWave2", 6.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject SpawnWave1()
    {
        // Spawn Enemies
        GameObject enemy1 = new GameObject();

        enemy1 = Instantiate(enemyResource[0], transform);

        return enemy1;
    }

    public GameObject SpawnWave2()
    {
        // Spawn Enemies
        GameObject enemy1 = new GameObject();

        enemy1 = Instantiate(enemyResource[0], transform);

        return enemy1;
    }
}
