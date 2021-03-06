﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform mCamTransform;
    public float mShakeDuration = 0.0f;

    public float mShakeAmount = 0.7f;
    public float mDecreaseFactor = 1.0f;

    Vector3 mOriginalPosition = new Vector3(0.0f,0.0f,0.0f);
    bool mShaking;

    public GameObject TPSpawner;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        mCamTransform = gameObject.transform;
        mOriginalPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(2.0f, 1.0f);
        }

        if (mShaking)
        {
            if (mShakeDuration > 0.0f)
            {
                Vector2 tmp = Random.insideUnitCircle;
                mCamTransform.position = mOriginalPosition + new Vector3(tmp.x, tmp.y, 0.0f) * mShakeAmount;

                mShakeDuration -= Time.deltaTime;
                mShakeAmount *= 0.98f;
            }
            else
            {
                mShakeDuration = 0.0f;
                mCamTransform.position = mOriginalPosition;
                mShaking = false;
            }
        }
        else
        {
            mCamTransform.position = mOriginalPosition;
        }

    }

    public void Shake(float _duration, float _magnitude = 1.0f)
    {
        if (_duration < 0.0f)
            return;

        mShakeDuration = _duration;
        mShakeAmount = _magnitude;

        mOriginalPosition = mCamTransform.position;
        mShaking = true;
    }
}
