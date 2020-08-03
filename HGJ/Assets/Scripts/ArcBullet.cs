using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ArcBullet : MonoBehaviour
{
    public Vector2 mVelocity;
    public float mGravity;

    Rigidbody2D mRB;

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        mVelocity.y -= mGravity * Time.deltaTime;

        mRB.velocity = mVelocity;
    }
}
