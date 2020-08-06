using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handleRain : MonoBehaviour
{

    Camera camera;
    float timer;
    int counter;
    public GameObject handle;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.6f) {

            Vector3 p = camera.ViewportToWorldPoint(new Vector3(Random.Range(0.05f, 0.75f), 1, 0));// + new Vector3(0,ypos/2,0);
            p.z = 0;
            counter++;
            Instantiate(handle, p, Quaternion.identity);
            timer = 0;
        }
        if (counter >15) {
            Destroy(gameObject);
        }
    }
}
