using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpiralFormation : MonoBehaviour
{
    public GameObject mVerticalBulletPrefab;
    public GameObject mHorizontalBulletPrefab;

    public float mSpawnInterval;
    public float mAmplitudeGrowthSpeed;
    public float mAngleGrowthSpeed;

    Transform mMyTransform;
    float mSpawnCounter;

    // Start is called before the first frame update
    void Start()
    {
        mSpawnCounter = mSpawnInterval;
        mMyTransform = GetComponent<Transform>();
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
        bullet = SpiralBulletFormation.Instantiate(mHorizontalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        MSpiralBulletHorizontal sp = bullet.GetComponent<MSpiralBulletHorizontal>();
        sp.mAmplitudeGrowth = mAmplitudeGrowthSpeed;
        sp.mAngleGrowth = mAngleGrowthSpeed;

        sp.transform.parent = mMyTransform;

        bullet = SpiralBulletFormation.Instantiate(mHorizontalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        sp = bullet.GetComponent<MSpiralBulletHorizontal>();
        sp.mAmplitudeGrowth = -mAmplitudeGrowthSpeed;
        sp.mAngleGrowth = mAngleGrowthSpeed;

        sp.transform.parent = mMyTransform;

        bullet = SpiralBulletFormation.Instantiate(mVerticalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        MSpiralBulletVertical spv = bullet.GetComponent<MSpiralBulletVertical>();
        spv.mAmplitudeGrowth = mAmplitudeGrowthSpeed;
        spv.mAngleGrowth = -mAngleGrowthSpeed;

        spv.transform.parent = mMyTransform;

        bullet = SpiralBulletFormation.Instantiate(mVerticalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        spv = bullet.GetComponent<MSpiralBulletVertical>();
        spv.mAmplitudeGrowth = -mAmplitudeGrowthSpeed;
        spv.mAngleGrowth = -mAngleGrowthSpeed;
       
        spv.transform.parent = mMyTransform;
    }
}
