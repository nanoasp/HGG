using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killparticlescript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(commitSudoku(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator commitSudoku(float wait)
    {
        yield return new WaitForSeconds(wait);

        Destroy(gameObject);
    }
}
