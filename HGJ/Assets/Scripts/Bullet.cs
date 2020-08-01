using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    public float speed;
    public float rotation;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 10.0f;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);

        if (timer < 0.0f)
            Destroy(this.gameObject);

        timer -= Time.deltaTime;
    }
}
