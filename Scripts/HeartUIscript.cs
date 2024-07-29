 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HeartUIscript : MonoBehaviour
{
    public GameObject heartPrefab; //This is the heart icon for the player's health
    public float heartSpacing = 110f; // Distance between heart UI icons

    // This is a child object/folder of the canvas to put all the generated hearts in for simplicity
    // (E.G. If we have 40 hearts we don't want it taking over the entire canvas bar.  
    public Transform heartDisplays;


    void Start()
    {
        createHearts();
    }


    void Update()
    {
        
    }

    public void createHearts()
    {
        Vector3 heartStartPosition = heartPrefab.GetComponent<RectTransform>().anchoredPosition; ; // Position of the first heart (Relative to heartDisplays)

        // Generate new hearts in the UI based on heart_count in the player script
        for (int i = 0; i < PlayerBehavior.heartCount; i++) //Heart Count is the variable that determines the health the player will have
        {
            GameObject newHeart = Instantiate(heartPrefab, heartDisplays);
            RectTransform rectTransform = newHeart.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = heartStartPosition + new Vector3(i * heartSpacing, 0, 0);
        }
    }
}



