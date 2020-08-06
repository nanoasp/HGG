using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MerlionTailSpawner : MonoBehaviour
{
    Transform mTransform;
    public GameObject mPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject bullet;
        bullet = MerlionTailSpawner.Instantiate(mPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0.0f)) as GameObject;
        bullet.transform.position = mTransform.position;
    }
}
