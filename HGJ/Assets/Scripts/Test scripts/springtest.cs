using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springtest : MonoBehaviour
{

    public float vel = 0;
    public float pos = 0;
    public float dampening = 0;
    public float freq = 0;
    public float oripos = 0;


    // Start is called before the first frame update
    void Start()
    {
        freq = Mathf.PI * freq;
        oripos = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position.x;
        NumbericSpring.spring(ref pos, ref vel, oripos, dampening,  freq, Time.deltaTime);
        transform.position = new Vector3(pos, transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.E)){
            vel = 20;
        }


    }
}
