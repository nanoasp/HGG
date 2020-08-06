using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterdropkillscript : MonoBehaviour
{
    public GameObject splash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -15.0f) {
            commitSudoku();
        }
    }


    public void commitSudoku()
    {
        GameObject sp = Instantiate(splash);
        sp.transform.position = transform.position;
        Destroy(gameObject);
    }


}
