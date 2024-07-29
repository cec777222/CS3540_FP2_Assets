using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePickups : MonoBehaviour
{
    public float rotationAmount = 45f;
    public float floatHeight = 0.5f;
    public float floatFrequency = 0.5f;

    public Color emissionColor = Color.yellow;
    public float emissionIntensity = 1.0f;
    public float pulseSpeed = 2f;

    private Material material;
    private Vector3 startPosition;
    private bool isAnimating = false; 

    void Start()
    {
        startPosition = transform.position;

        material = GetComponent<Renderer>().material;
        material.EnableKeyword("_EMISSION");
    }
    
    void OnEnable()
    {
        isAnimating = true;
    }

    void OnDisable()
    {
        isAnimating = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isAnimating) return;

        
        transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);

        float floatingObjects = startPosition.y + Mathf.Abs(Mathf.Sin(Time.time * floatFrequency)) * floatHeight;
        transform.position = new Vector3(transform.position.x, floatingObjects, transform.position.z);

        float emission = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f * emissionIntensity;
        Color finalColor = emissionColor * Mathf.LinearToGammaSpace(emission);
        material.SetColor("_EmissionColor", finalColor);
    }
}