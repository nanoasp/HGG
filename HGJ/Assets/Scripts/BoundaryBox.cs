using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.tag == "PlayerWater" || col.gameObject.tag == "EnemyAttack" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "PlayerAttack")
       {
            Destroy(col.gameObject);
       }
    }
}
