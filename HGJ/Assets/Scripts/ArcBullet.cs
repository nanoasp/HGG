using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ArcBullet : MonoBehaviour
{
    public Vector2 mVelocity;
    public float mGravity;

    Rigidbody2D mRB;
    float hp;

    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        mVelocity.y -= mGravity * Time.deltaTime;
        hp = 5;
        mRB.velocity = mVelocity;
        if (hp <= 0) { 
        
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerWater")
        {
            col.gameObject.GetComponent<waterdropkillscript>().commitSudoku();
            hp--;
        }
        if (col.gameObject.tag == "PlayerAttack")
        {
            Destroy(col.gameObject);
            hp--;
        }
    }
}
