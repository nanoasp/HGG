using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChargeUI : MonoBehaviour
{
    Image box;
    public PlayerController2D Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        box.color = new Color(1,1,1, 0.5f + Player.mBeamCounter / 200);
        box.fillAmount = Player.mBeamCounter / 100;



    }
}
