using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CutSceneText : MonoBehaviour
{
    public int mTextSlot = 0;
    public GameObject mText2;

    public float mLife = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mLife <= 0.0f || Input.GetKey(KeyCode.Space))
        {
            if(mTextSlot == 0)
            {
                mText2.GetComponent<MeshRenderer>().enabled = true;
                GetComponent<MeshRenderer>().enabled = false;
                mLife = 4.0f;
            }
            else
            {
                //change scene
                SceneManager.LoadScene("Level1");
            }
        }

        mLife -= Time.deltaTime;
    }
}
