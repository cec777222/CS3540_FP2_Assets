using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 30;

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0){
            currentHealth -= damageAmount;
        }
        if(currentHealth <= 0){
            PlayerDies();
        }

        Debug.Log("Current health: " + currentHealth);
    }

    public void AddHealth(int healthAmount)
    {
        currentHealth += healthAmount;
        
        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }

        Debug.Log("Health added! Current health: " + currentHealth);
    }

    void PlayerDies(){
        Debug.Log("Player is dead...");
    }
}