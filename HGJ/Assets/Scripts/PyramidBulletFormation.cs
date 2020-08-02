using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidBulletFormation : MonoBehaviour
{
    public GameObject mBulletPrefab;

    public GameObject mSpawner1;
    public GameObject mSpawner2;
    public GameObject mSpawner3;
    public GameObject mSpawner4;

    public int mIterations;
    public float mProjectileSpeed = 5;
    public float mSpawnInterval = 1.0f;

    float mSpawnCounter;
    int mIterationCounter = 0;

    Transform mMyTransform;
    FormationBulletSpawner mFBSpawner1;
    FormationBulletSpawner mFBSpawner2;
    FormationBulletSpawner mFBSpawner3;
    FormationBulletSpawner mFBSpawner4;

    // Start is called before the first frame update
    void Start()
    {
        mMyTransform = GetComponent<Transform>();
        mFBSpawner1 = mSpawner1.GetComponent<FormationBulletSpawner>();
        mFBSpawner2 = mSpawner2.GetComponent<FormationBulletSpawner>();
        mFBSpawner3 = mSpawner3.GetComponent<FormationBulletSpawner>();
        mFBSpawner4 = mSpawner4.GetComponent<FormationBulletSpawner>();

        mSpawnCounter = mSpawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (mIterationCounter == mIterations || mIterationCounter >= 3)
            Destroy(gameObject);

        if (mSpawnCounter <= mSpawnInterval)
        {
            mSpawnCounter += Time.fixedDeltaTime;
            return;
        }
        else
            mSpawnCounter = 0.0f;

        GameObject bullet;
        Vector3 forward = new Vector3(mMyTransform.forward.x, mMyTransform.forward.y, mMyTransform.forward.z);
        forward.Normalize();

        switch (mIterationCounter)
        {
            case 0:
                bullet = FormationBulletSpawner.Instantiate(mBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                bullet.transform.position = mMyTransform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = forward * mProjectileSpeed;
                break;
            case 1:
                mFBSpawner1.Spawn(mBulletPrefab, mProjectileSpeed);
                mFBSpawner2.Spawn(mBulletPrefab, mProjectileSpeed);
                break;
            case 2:
                bullet = FormationBulletSpawner.Instantiate(mBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
                bullet.transform.position = mMyTransform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = forward * mProjectileSpeed;

                mFBSpawner3.Spawn(mBulletPrefab, mProjectileSpeed);
                mFBSpawner4.Spawn(mBulletPrefab, mProjectileSpeed);
                break;
        }

        mIterationCounter += 1;
    }
}
