using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public enum PickupType
    {
        Salmon,
        Rice,
        Nori
    }

    public PickupType pickupType; // Make sure to assign each of the type in the Inspector for each pickup!!
    public static int[] pickupCount = new int[3];
    private static bool[] collectedPickups = new bool[3];
    private LevelManager levelManager;

    void Start()
    {
        pickupCount[(int)pickupType]++;
        Debug.Log("Pickup count for " + pickupType + ": " + pickupCount[(int)pickupType]);

        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " triggered " + pickupType + " pickup!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        pickupCount[(int)pickupType]--;
        Debug.Log("Pickups remaining for " + pickupType + ": " + pickupCount[(int)pickupType]);

        if (pickupCount[(int)pickupType] <= 0)
        {
            collectedPickups[(int)pickupType] = true;
            Debug.Log(pickupType + " pickups collected!");
        }

        if (AllPickupsCollected())
        {
            Debug.Log("You Win!");
            if (levelManager != null)
            {
                levelManager.LevelBeat();
            }
        }
    }

    private bool AllPickupsCollected()
    {
        foreach (bool collected in collectedPickups)
        {
            if (!collected) return false;
        }
        return true;
    }
}
