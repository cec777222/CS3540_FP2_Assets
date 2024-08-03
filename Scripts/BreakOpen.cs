using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOpen : MonoBehaviour
{
    public float explosionForce = 100f;
    public float explosionRadius = 5f;
    public GameObject explosionEffectPrefab;
    public GameObject lootPrefab;
    private bool isDestroyed = false;
    private float destroyDelay = 0.1f;


    private void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.gameObject.CompareTag("Projectile"))
        {
            isDestroyed = true;

            StartCoroutine(DelayedDestruction());
        }
    }

    private IEnumerator DelayedDestruction()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (lootPrefab != null)
        {
            Vector3 lootPosition = transform.position + Vector3.up * 0.5f; 
            Instantiate(lootPrefab, lootPosition, Quaternion.identity);
        }

        ApplyExplosionForce();

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }

    private void ApplyExplosionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb != GetComponent<Rigidbody>())
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

}
