using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRBackgroundSpawnBlock : MonoBehaviour
{
    public GameObject mInitialObj1;
    public GameObject mInitialObj2;
    public GameObject mInitialObj3;
    
    public float mTreeVelocity;
    public float mBuildingVelocity;
    public float mBackdropVelocity;

    // Start is called before the first frame update
    void Start()
    {
        mInitialObj1.GetComponent<Rigidbody2D>().velocity = new Vector2(mTreeVelocity, 0.0f);
        mInitialObj2.GetComponent<Rigidbody2D>().velocity = new Vector2(mBuildingVelocity, 0.0f);
        mInitialObj3.GetComponent<Rigidbody2D>().velocity = new Vector2(mBackdropVelocity, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
