using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerlionSingleBeam : MonoBehaviour
{
    bool mGrow = true;
    Transform mTransform;
    public float mLaserSize;
    public float mGrowthSpeed;

    float mSizeCounter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<Transform>();    
    }

    // Update is called once per frame
    void Update()
    {
        float coefficient = 0.0f;
        
        if (mGrow)
            coefficient = 1.0f;
        else
            coefficient = -1.0f;

        mSizeCounter += coefficient * mGrowthSpeed * Time.deltaTime;

        if (mSizeCounter >= mLaserSize)
        {
            mSizeCounter = mLaserSize;
            mGrow = false;
        }
        else if (mSizeCounter < 0.0f)
            mSizeCounter = 0.0f;

        mTransform.localScale = new Vector3(3.0f, mSizeCounter, 1.0f);

        if (!mGrow && mSizeCounter <= 0.0f)
            Destroy(this);
    }
}
