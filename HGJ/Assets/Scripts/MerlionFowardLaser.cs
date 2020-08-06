using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MerlionFowardLaser : MonoBehaviour
{
    Transform mTransform;
    Rigidbody2D mRB;
    Vector3 position;

    public bool mActive = false;
    
    float mSizeCounter = 0.0f;
    float mSize = 3.0f;
    float mLife = 4.0f;

    public float mVelocity;

    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<Transform>();
        position = new Vector3(mTransform.localPosition.x, mTransform.localPosition.y, -1.0f);

        mRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mActive)
            return;

        mSizeCounter += 10.0f * Time.deltaTime;

        if (mSizeCounter >= mSize)
            mSizeCounter = mSize;

        mTransform.localScale = new Vector3(mSize, mSizeCounter, 0.0f);

        mLife -= Time.deltaTime;

        if (mLife <= 0.0f)
            Reset();
    }

    public void Activate()
    {
        mActive = true;
        mRB.velocity = new Vector2(mVelocity, 0.0f);
    }

    void Reset()
    {
        mActive = false;
        mSizeCounter = 0.0f;
        mLife = 4.0f;
        mTransform.localScale = new Vector3(mSize, 0.0f, 0.0f);
        mTransform.localPosition = new Vector3(position.x, position.y, -1.0f);
        mRB.velocity = new Vector2(0.0f, 0.0f);
    }
}
