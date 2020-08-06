using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants
{
    public const float X_RIGHT_R = 5.5f;
    public const float X_RIGHT_M = 4.5f;
    public const float X_RIGHT_L = 3.5f;
    public const float X_MIDDLE = 0.0f;
    public const float X_LEFT = -1.0f;
    public const float Y_HEAVEN = 5.2f;

    public const float Y_TOP = 4.0f;
    public const float Y_MIDDLE_T = 2.0f;
    public const float Y_MIDDLE = 0.0f;
    public const float Y_MIDDLE_B = -2.0f;
    public const float Y_BOTTOM = -4.0f;
    public const float Y_HELL = -5.2f;
}

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] enemyResource;

    void Start()
    {
        StartCoroutine(SpawnWave(enemyResource[0], 2.0f, new Vector2(transform.position.x, Constants.Y_MIDDLE_B)));
        StartCoroutine(SpawnWave(enemyResource[0], 12.0f, new Vector2(transform.position.x, Constants.Y_MIDDLE_B)));
        StartCoroutine(SpawnWave(enemyResource[1], 18.0f, new Vector2(transform.position.x, Constants.Y_BOTTOM)));
        StartCoroutine(SpawnWave(enemyResource[2], 18.0f, new Vector2(Constants.X_RIGHT_M, Constants.Y_HEAVEN)));
        StartCoroutine(SpawnWave(enemyResource[2], 18.0f, new Vector2(Constants.X_RIGHT_R, Constants.Y_HEAVEN)));
        StartCoroutine(SpawnWave(enemyResource[1], 25.0f, new Vector2(transform.position.x, Constants.Y_TOP)));
        StartCoroutine(SpawnWave(enemyResource[3], 30.0f, new Vector2(Constants.X_RIGHT_M, Constants.Y_HELL)));
        StartCoroutine(SpawnWave(enemyResource[3], 30.0f, new Vector2(Constants.X_RIGHT_R, Constants.Y_HELL)));
        StartCoroutine(SpawnWave(enemyResource[0], 36.0f, new Vector2(transform.position.x, Constants.Y_MIDDLE_B)));
        StartCoroutine(SpawnWave(enemyResource[0], 46.0f, new Vector2(transform.position.x, Constants.Y_MIDDLE_B)));
        StartCoroutine(SpawnWave(enemyResource[1], 52.0f, new Vector2(transform.position.x, Constants.Y_BOTTOM)));
        StartCoroutine(SpawnWave(enemyResource[2], 52.0f, new Vector2(Constants.X_RIGHT_M, Constants.Y_HEAVEN)));
        StartCoroutine(SpawnWave(enemyResource[2], 58.0f, new Vector2(Constants.X_RIGHT_R, Constants.Y_HEAVEN)));
        StartCoroutine(SpawnWave(enemyResource[1], 65.0f, new Vector2(transform.position.x, Constants.Y_TOP)));
        StartCoroutine(SpawnWave(enemyResource[3], 70.0f, new Vector2(Constants.X_RIGHT_M, Constants.Y_HELL)));
        StartCoroutine(SpawnWave(enemyResource[3], 70.0f, new Vector2(Constants.X_RIGHT_R, Constants.Y_HELL)));
        StartCoroutine(SpawnWave(enemyResource[4], 78.0f, new Vector2(Constants.X_RIGHT_R, Constants.Y_HELL)));

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnWave(GameObject go, float wait, Vector2 pos)
    {
        yield return new WaitForSeconds(wait);

        Instantiate(go, new Vector3(pos.x, pos.y), Quaternion.identity);
    }
}
