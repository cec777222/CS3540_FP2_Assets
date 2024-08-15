using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionWarning : MonoBehaviour
{
    public float initialBlinkInterval = 1.0f; // Initial interval between blinks in seconds
    public float blinkAcceleration = 0.1f; // How much faster the blinking gets per second

    private Renderer objectRenderer;
    private Color originalColor;
    private float blinkTimer;
    private float currentBlinkInterval;

    private bool willExplode;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        currentBlinkInterval = initialBlinkInterval;
        blinkTimer = currentBlinkInterval;

        willExplode = false;
    }

    void Update()
    {
        willExplode = SnowManBehavior.StartExplosionWarning();
        if (willExplode)
        {
            blinkTimer -= Time.deltaTime;

            if (blinkTimer <= 0)
            {
                // Toggle the color between red and the original color
                if (objectRenderer.material.color == originalColor)
                {
                    objectRenderer.material.color = Color.red;
                }
                else
                {
                    objectRenderer.material.color = originalColor;
                }

                // Reset the timer and decrease the interval
                blinkTimer = currentBlinkInterval;
                currentBlinkInterval = Mathf.Max(0.1f, currentBlinkInterval - blinkAcceleration * Time.deltaTime);
            }
        }
    }
}