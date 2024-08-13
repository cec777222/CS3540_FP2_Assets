using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button legacyButton; 
    public Text buttonText; 

    void Start()
    {
        // Set the button text when the game starts
        buttonText.text = "WASD to move, kill enemies and collect their pickups to win the game. Good Luck!";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            HideButton();
        }
    }

    void HideButton()
    {
        // Hide the button
        legacyButton.gameObject.SetActive(false);
    }
}
