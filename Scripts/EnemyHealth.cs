using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip enemyHitSFX;
    public Slider enemyHealthSlider;

    private PlayerControllerFixed playerControllerFixedx;

    void Awake()
    {
        enemyHealthSlider = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        currentHealth = startingHealth;
        enemyHealthSlider.value = currentHealth;
        playerControllerFixedx = player.GetComponent<PlayerControllerFixed>();

    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            enemyHealthSlider.value = currentHealth;
        }

        if(currentHealth <= 0){
            //dead
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
        AudioSource.PlayClipAtPoint(enemyHitSFX, transform.position);
        TakeDamage(playerControllerFixedx.playerDamage);
        }
    }

}