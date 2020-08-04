using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BackgroundSpawnBlock : MonoBehaviour
{
    public GameObject mBGObj1;
    public GameObject mBGObj2;
    public GameObject mSpawnBlock;
    public GameObject mInitialObj;

    public float mVelocity;

    Transform mSpawnTransform;
    bool mSpawn1 = false;

    // Start is called before the first frame update
    void Start()
    {
        mSpawnTransform = mSpawnBlock.GetComponent<Transform>();    
        mInitialObj.GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "BackgroundSpawnBlock")
        {
            GameObject mTemp;

            if(mSpawn1)
            {
                mTemp = BackgroundSpawnBlock.Instantiate(mBGObj1, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
                mTemp.transform.position = mSpawnTransform.position;
                mTemp.GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);

                mSpawn1 = false;
            }
            else
            {
                mTemp = BackgroundSpawnBlock.Instantiate(mBGObj2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 0)) as GameObject;
                mTemp.transform.position = mSpawnTransform.position;
                mTemp.GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);
                mSpawn1 = true;
            }
        }
    }
}
