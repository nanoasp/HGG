using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    public GameObject creditsPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame() {
        SceneManager.LoadScene("IntroCutscene");

    }

    public void toggleCredits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);

    }

    public void killGame()
    {
        Application.Quit();

    }




}
