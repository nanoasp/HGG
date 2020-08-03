using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralBulletFormation : MonoBehaviour
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
        SpiralBulletHorizontal sp = bullet.GetComponent<SpiralBulletHorizontal>();
        sp.mAmplitudeGrowth = mAmplitudeGrowthSpeed;
        sp.mAngleGrowth = mAngleGrowthSpeed;
        sp.mStartingPosition = mMyTransform.position;

        bullet = SpiralBulletFormation.Instantiate(mHorizontalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        sp = bullet.GetComponent<SpiralBulletHorizontal>();
        sp.mAmplitudeGrowth = -mAmplitudeGrowthSpeed;
        sp.mAngleGrowth = mAngleGrowthSpeed;
        sp.mStartingPosition = mMyTransform.position;

        bullet = SpiralBulletFormation.Instantiate(mVerticalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        SpiralBulletVertical spv = bullet.GetComponent<SpiralBulletVertical>();
        spv.mAmplitudeGrowth = mAmplitudeGrowthSpeed;
        spv.mAngleGrowth = -mAngleGrowthSpeed;
        spv.mStartingPosition = mMyTransform.position;

        bullet = SpiralBulletFormation.Instantiate(mVerticalBulletPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mMyTransform.position;
        spv = bullet.GetComponent<SpiralBulletVertical>();
        spv.mAmplitudeGrowth = -mAmplitudeGrowthSpeed;
        spv.mAngleGrowth = -mAngleGrowthSpeed;
        spv.mStartingPosition = mMyTransform.position;
    }
}
