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

    public float mSpeed = 5.0f;
    public int mHp = 10;

    // Start is called before the first frame update
    void Start()
    {
        mState = ANIKI_STATES.SANDWICH;
        mMarkerTransform = AnikiMarker.GetComponent<Transform>();
        mTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 marker_pos = mMarkerTransform.position;
        Vector2 my_pos = mTransform.position;

        Vector2 dir = (marker_pos - my_pos);
        //dir.Normalize();

        mTransform.position = new Vector2(mTransform.position.x, mTransform.position.y) + (dir * mSpeed * Time.fixedDeltaTime);
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

    void OnTriggerEnter2d(Collision2D col)
    {
        if(col.gameObject.tag == "EnemyAttack")
        {
            mHp -= 1;
            mHp = mHp < 0 ? 0 : mHp;

            Destroy(col.gameObject);
        }
    }
}
