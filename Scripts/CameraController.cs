using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset; 

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        // This will track where the camera is in relation to the player at the start of the game.
        offset = transform.position - player.transform.position; 
    }

    void Update()
    {
        if (!LevelManager.isGameOver && player != null)
        {
            //Move the camera to follow the player at the same offset position as specified in Start
            transform.position = player.transform.position + offset;
        }
    }
}


