using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform mCamTransform;
    public float mShakeDuration = 0.0f;
    
    public float mShakeAmount = 0.7f;
    public float mDecreaseFactor = 1.0f;

    Vector3 mOriginalPosition;
    bool mShaking;

    public GameObject TPSpawner;
    public GameObject Player;

    //camera variables
    float min_zoom = -10.0f;
    float max_zoom = 8;
    float damping = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        mCamTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p0 = TPSpawner.GetComponent<Spawner>().midpoint;
        Vector3 p1 = Player.transform.position;

        Vector3 distance = p0 - p1;
        Vector2 distance2 = new Vector2(distance.x, distance.y);
        float dist = Mathf.Min(distance2.magnitude * damping, max_zoom);

        Vector3 midpoint = (p0 + p1) * 0.5f;
        float new_z = min_zoom - 1.0f * dist;

        Vector3 new_pos = new Vector3(midpoint.x, midpoint.y, new_z);

        if (Input.GetKeyDown(KeyCode.T))
        {
            Shake(2.0f, 1.0f);
        }

        if (mShaking)
        {
            if (mShakeDuration > 0.0f)
            {
                Vector2 tmp = Random.insideUnitCircle;
                mCamTransform.position = new_pos + new Vector3(tmp.x, tmp.y, 0.0f) * mShakeAmount;

                mShakeDuration -= Time.deltaTime;
                mShakeAmount *= 0.98f;
            }
            else
            {
                mShakeDuration = 0.0f;
                //mCamTransform.position = mOriginalPosition;
                mShaking = false;
            }
        }
        else
        {
            mCamTransform.position = new_pos;
        }

    }

    public void Shake(float _duration, float _magnitude = 1.0f)
    {
        if (_duration < 0.0f)
            return;

        mShakeDuration = _duration;
        mShakeAmount = _magnitude;

        //mOriginalPosition = mCamTransform.position;
        mShaking = true;
    }
}
