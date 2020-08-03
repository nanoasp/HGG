using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSpiralBulletVertical : MonoBehaviour
{
    public float mAmplitudeGrowth;
    public float mAngleGrowth;

    float mAngle = 0.0f;
    float mAmplitude = 0.0f;

    Transform mMyTransform;
    public Vector2 mStartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        mMyTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Cos(mAngle) * mAmplitude;
        float x = Mathf.Sin(mAngle) * mAmplitude;

        mMyTransform.localPosition = mStartingPosition + new Vector2(x, y);
        mAngle += Time.deltaTime * mAngleGrowth;
        mAmplitude += Time.deltaTime * mAmplitudeGrowth;
    }
}
