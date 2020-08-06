using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolleybehaviour : MonoBehaviour
{

    public List<GameObject> myBullets;
    public GameObject priBullets;
    public GameObject arcBullets;

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
    public void ShootPri()
    {
        GameObject newbullet = Instantiate(priBullets);
        newbullet.transform.position = transform.position;
        //set speed maybe?
        //newbullet.GetComponent<>().speed = somespeed;

        myBullets.Add(newbullet);
        variationFlag++;
        if (variationFlag >= myBulletsVariations.Count)
        {
            variationFlag = 0;
        }
    }
    public void ShootArc()
    {
        GameObject newbullet = Instantiate(arcBullets);
        newbullet.transform.position = transform.position;
        //set speed maybe?
        //newbullet.GetComponent<>().speed = somespeed;

        myBullets.Add(newbullet);
        variationFlag++;
        if (variationFlag >= myBulletsVariations.Count)
        {
            variationFlag = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerWater")
        {
            col.gameObject.GetComponent<waterdropkillscript>().commitSudoku();
        }
        if (col.gameObject.tag == "PlayerAttack")
        {
            Destroy(col.gameObject);
        }
    }
}
