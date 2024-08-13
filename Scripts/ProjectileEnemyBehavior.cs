using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform player;

    // For Player interaction.
    public float detectDistance = 10f;
    public int startingHealth = 30; // Assuming a player's attack is 10 damage.
    int currentHealth;
    private bool playerDetected;
    private bool currentlyShooting;


    // For enemy death and loot pickup.
    public AudioClip deadSFX;
    public AudioClip shootSFX;
    public GameObject[] lootPrefabs;
    public AudioClip enemyHitSFX;

    public static int projectileEnemyCount;

    private bool isDead = false;

    //private PlayerControllerFixed playerControllerFixedx;

    // Start is called before the first frame update
    void Start()
    {

        // Finds the player object.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Setting booleans.
        playerDetected = false;
        currentlyShooting = false;

        // Sets health.
        currentHealth = startingHealth;

        projectileEnemyCount += 1;

        //playerControllerFixedx = player.GetComponent<PlayerControllerFixed>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (detectDistance > distance)
            {
                transform.LookAt(player);
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }

            if (playerDetected && !currentlyShooting)
            {
                RepeatShooting();
                currentlyShooting = true;
            }
            else if (!playerDetected && currentlyShooting)
            {
                CancelInvoke("Shoot");
                currentlyShooting = false;
            }
        }
    }

    void RepeatShooting()
    {
        InvokeRepeating("Shoot", 1f, 3f);
    }

    void Shoot()
    {
        AudioSource.PlayClipAtPoint(shootSFX, transform.position);
        GameObject projectile;
        projectile = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
    }

    public void EnemyAttacked(int damageTaken)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageTaken;
        }
        if (currentHealth <= 0)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        //AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        LevelManager.enemyKillCount += 1;


        if (lootPrefabs.Length > 0 && !isDead)
        {
            isDead = true;
            int randomIndex = Random.Range(0, lootPrefabs.Length);
            Instantiate(lootPrefabs[randomIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeaponAttackHitBox"))
        {
            Debug.Log("HIT");
            AudioSource.PlayClipAtPoint(enemyHitSFX, transform.position);
            EnemyAttacked(PlayerControllerFixed.playerDamage);
        }
    }
}