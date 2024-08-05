using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookFixed : MonoBehaviour
{
    public float mouseSens = 200;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;

        // Rotate only on the horizontal axis (Y-axis)
        transform.Rotate(Vector3.up * moveX);
    }
}
