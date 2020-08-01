using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AnikiMarker : MonoBehaviour
{
    public enum ANIKI_STATES
    {
        SANDWICH = 0,
        FRONTAL,
    };

    ANIKI_STATES mState;
    Transform mTransform;

    public Vector2 mSandwichPosition;
    public Vector2 mFrontalPosition;

    // Start is called before the first frame update
    void Start()
    {
        mState = ANIKI_STATES.SANDWICH;
        mTransform = GetComponent<Transform>();
        mSandwichPosition = mTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        if(mState == ANIKI_STATES.SANDWICH)
        {
            mTransform.localPosition = new Vector2(mFrontalPosition.x, mFrontalPosition.y);
            mState = ANIKI_STATES.FRONTAL;
        }
        else
        {
            mTransform.localPosition = new Vector2(mSandwichPosition.x, mSandwichPosition.y);
            mState = ANIKI_STATES.SANDWICH;
        }
    }
}
