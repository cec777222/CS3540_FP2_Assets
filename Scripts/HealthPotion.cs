using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healthAmount = 5;
    public AudioClip lootSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            if (lootSFX != null)
            {
                AudioSource.PlayClipAtPoint(lootSFX, transform.position);
            }

            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthAmount);
            }

            Destroy(gameObject, 0.5f);
        }
    }
}
