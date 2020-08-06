using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerlionTailProjectile : MonoBehaviour
{
    Transform mTransform;
    public float mVelocity;

    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mTransform.position.y < -2.5f)
        {
            float pos_y = mTransform.position.y + Time.deltaTime * 3.0f;

            if (pos_y >= -2.5f)
                pos_y = -2.5f;

            mTransform.position = new Vector3(mTransform.position.x, pos_y, mTransform.position.z);

            return;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(mVelocity, 0.0f);
    }
}
