using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookFixed : MonoBehaviour
{
    public float mouseSens = 200;

    public float maxXRotation = 35; // Maximum rotation in degrees on the X-axis
    public float minRotation = -25;
    private float xRotation = 0f; // Current rotation around the X-axis

    private Transform cameraTransform; 
    private float initialYRotation;
    private float initialZRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = Camera.main.transform;


        Vector3 initialRotation = cameraTransform.localRotation.eulerAngles;
        xRotation = initialRotation.x;
        initialYRotation = initialRotation.y;
        initialZRotation = initialRotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * moveX);

        // Update and clamp the X-axis rotation
        xRotation -= moveY;
        xRotation = Mathf.Clamp(xRotation, -minRotation, maxXRotation);

        // Apply the clamped rotation to the camera, preserving the initial Y and Z rotation
        cameraTransform.localRotation = Quaternion.Euler(xRotation, initialYRotation, initialZRotation);
    }
}
