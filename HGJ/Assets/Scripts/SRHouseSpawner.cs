using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRHouseSpawner : MonoBehaviour
{
    public GameObject mHouse1;
    public GameObject mHouse2;

    public float mSpawnInterval = 0.5f;
    public float mVelocity;

    float mSpawnCounter;
    Transform mTransform;

    // Start is called before the first frame update
    void Start()
    {
        mSpawnCounter = mSpawnInterval;
        mTransform = GetComponent<Transform>();
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

        GameObject obj;
        GameObject prefab;

        float val = Random.Range(0.0f, 100.0f);

        if(val >= 50.0f)
        {
            prefab = mHouse1;
        }   
        else
        {
            prefab = mHouse2;
        }

        obj = SRHouseSpawner.Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
        obj.transform.position = mTransform.position;
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);
    }
}
