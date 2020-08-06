using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerlionAcrBulletSpawner : MonoBehaviour
{
    public GameObject mBulletPrefab;
    public Vector2 mVelocityRange;
    public float mGravityScale;

    Transform mMyTransform;

    // Start is called before the first frame update
    void Start()
    {
        mMyTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        GameObject bullet;

        bullet = MerlionAcrBulletSpawner.Instantiate(mBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        ArcBullet ab = bullet.GetComponent<ArcBullet>();

        float x = Random.Range(0.0f, mVelocityRange.x);
        float y = Random.Range(0.0f, mVelocityRange.y);
        ab.mVelocity = new Vector2(-(x + 1.0f), (y + 2.0f));
        ab.mGravity = mGravityScale;
    }
}
