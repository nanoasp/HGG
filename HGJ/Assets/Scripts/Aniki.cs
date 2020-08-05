using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Timeline;

public class Aniki : MonoBehaviour
{
    public enum ANIKI_STATES
    {
        SANDWICH = 0,
        FRONTAL,
    };

    ANIKI_STATES mState;
    public GameObject AnikiMarker;
    Transform mTransform;
    Transform mMarkerTransform;
    SpriteRenderer mSprite;

    public float mSpeed = 5.0f;
    public int mHp = 10;
    public float mResetInterval = 5.0f;

    float mResetCounter = 0.0f;
    bool mReset = false;

    // Start is called before the first frame update
    void Start()
    {
        mState = ANIKI_STATES.SANDWICH;
        mMarkerTransform = AnikiMarker.GetComponent<Transform>();
        mTransform = GetComponent<Transform>();
        mSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 marker_pos = mMarkerTransform.position;
        Vector2 my_pos = mTransform.position;

        Vector2 dir = (marker_pos - my_pos);

        mTransform.position = new Vector2(mTransform.position.x, mTransform.position.y) + (dir * mSpeed * Time.fixedDeltaTime);

        if(mHp == 0)
        {
            mSprite.color = new Color(mSprite.color.r, mSprite.color.g, mSprite.color.b, 0.0f);
        }
        else if(mHp <= 2)
        {
            mSprite.color = new Color(mSprite.color.r, mSprite.color.g, mSprite.color.b, 0.25f);
        }
        else if (mHp <= 5)
        {
            mSprite.color = new Color(mSprite.color.r, mSprite.color.g, mSprite.color.b, 0.5f);
        }
        else if (mHp <= 10)
        {
            mSprite.color = new Color(mSprite.color.r, mSprite.color.g, mSprite.color.b, 1.0f);
        }

        if(mReset)
        {
            mResetCounter += Time.deltaTime;

            if(mResetCounter >= mResetInterval)
            {
                mReset = false;
                mResetCounter = 0.0f;
                mHp = 10;
            }
        }
    }

    public void Toggle()
    {
        if (mState == ANIKI_STATES.SANDWICH)
        {
            mTransform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
            mState = ANIKI_STATES.FRONTAL;
        }
        else
        {
            mTransform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
            mState = ANIKI_STATES.SANDWICH;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "EnemyAttack")
        {
            if (!mReset && mHp > 0)
            {
                mHp -= 1;
                mHp = mHp < 0 ? 0 : mHp;

                Destroy(col.gameObject);

                if (mHp == 0)
                    mReset = true;
            }
        }
    }
}
