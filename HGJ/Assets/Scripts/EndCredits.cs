using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    public float mTimer = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(mTimer <= 0.0f || Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu");
        }

        mTimer -= Time.deltaTime;
    }
}
