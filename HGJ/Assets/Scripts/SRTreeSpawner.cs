using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRTreeSpawner : MonoBehaviour
{
    public GameObject mTree1;
    public GameObject mTree2;
    public GameObject mTree3;
    
    int mCounter = 0;

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

        switch(mCounter)
        {
            case 0:
                prefab = mTree1;
                break;
            case 1:
                prefab = mTree2;
                break;
            default: case 2:
                prefab = mTree3;
                break;
        }

        obj = SRTreeSpawner.Instantiate(prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
        obj.transform.position = mTransform.position;
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);

        mCounter += 1;
        
        if (mCounter >= 3)
            mCounter = 0;

    }
}
