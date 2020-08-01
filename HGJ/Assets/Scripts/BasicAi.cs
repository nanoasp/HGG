using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAi : MonoBehaviour
{
    public int startHp;
    public GameObject dropResource;
    int hp;

    public Vector2 velocity;
    public float speed;
    public float rotation;
    public float flipTimer;
    public bool jump;
    public float jumpVelocity;
    public bool spin;
    public float spinVelocity;

    float flipTime;
    // Start is called before the first frame update
    void Start()
    {
        
        hp = startHp;
        flipTime = flipTimer;

        if (jump)
            velocity.y += jumpVelocity;

        Invoke("Destroy", 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (flipTime < 0.0f)
        {
            velocity.y *= -1.0f;
            flipTime = flipTimer;
        }

        if (hp < 0)
        {
            Destroy();
            Instantiate(dropResource, transform);
        }

        if (spin)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            rotation += spinVelocity;
        }

        transform.position += new Vector3(velocity.x, velocity.y) * speed * Time.deltaTime;
        flipTime -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "PlayerAttack")
        {
            hp -= 1;
        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
