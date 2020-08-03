using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Material mMaterial;

    float mThreshold;
    bool mFadeIn;
    bool mFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        mThreshold = 0.0f;
        mFadeOut = false;
        mFadeIn = true;
    }

    public void FadeOut()
    {
        mThreshold = 1.0f;
        mFadeOut = true;
        mFadeIn = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    FadeIn();
        //}
        //
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    FadeOut();
        //}

        if(mFadeOut)
        {
            mThreshold -= Time.fixedDeltaTime/4;
            mMaterial.SetFloat("Threshold", mThreshold);

            if (mThreshold < 0.0f)
                mFadeOut = false;
        }
        else if(mFadeIn)
        {
            mThreshold += Time.fixedDeltaTime/4;
            mMaterial.SetFloat("Threshold", mThreshold);

            if (mThreshold > 1.0f)
                mFadeIn = false;
        }
    }
}
