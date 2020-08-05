using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBulletSpawner : MonoBehaviour
{
    public GameObject mBulletPrefab;
    public GameObject mBulletPrefab2;
    public Vector2 mInitialSpeed;
    public float mGravityScale;

    public float mSpawnInterval;
    float mSpawnCounter;
    Transform mMyTransform;

    // Start is called before the first frame update
    void Start()
    {
        mSpawnCounter = mSpawnInterval;
        mMyTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mSpawnCounter < mSpawnInterval)
        {
            mSpawnCounter += Time.deltaTime;
            return;
        }
        else
            mSpawnCounter = 0.0f;

        GameObject bullet;
        if (mInitialSpeed.y > 1)
        {
            mInitialSpeed.y--;
        }
        else {
            mInitialSpeed.y = 5;

        }
        if (mInitialSpeed.y %2 ==0) {
            bullet = ArcBulletSpawner.Instantiate(mBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;

        }else 
        { 
        bullet = ArcBulletSpawner.Instantiate(mBulletPrefab2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        }
        bullet.transform.position = mMyTransform.position;
        ArcBullet ab = bullet.GetComponent<ArcBullet>();
        ab.mVelocity = mInitialSpeed;
        ab.mGravity = mGravityScale;
    }
}
