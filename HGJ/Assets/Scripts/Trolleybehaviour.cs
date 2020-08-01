using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolleybehaviour : MonoBehaviour
{

    public List<GameObject> myBullets;

    public List<GameObject> myBulletsVariations;
    int variationFlag;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootBullet() {
        GameObject newbullet = Instantiate(myBulletsVariations[variationFlag]);
        newbullet.transform.position = transform.position;
        //set speed maybe?
        //newbullet.GetComponent<>().speed = somespeed;
        
        myBullets.Add(newbullet);
        variationFlag++;
        if (variationFlag >= myBulletsVariations.Count) {
            variationFlag = 0;
        }
    }
}
