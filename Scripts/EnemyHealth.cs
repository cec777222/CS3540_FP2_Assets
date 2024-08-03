using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip enemyHitSFX;
    public Slider healthSlider;

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0){
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0){
            //dead
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            AudioSource.PlayClipAtPoint(enemyHitSFX, transform.position);
            TakeDamage(10);
        }
    }
}