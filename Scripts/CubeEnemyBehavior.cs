using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeEnemyBehavior : MonoBehaviour
{
    //For player detection and movement.
    public Transform player;

    public float moveSpeed = 1;
    public float minDistance = 1.5f;
    public float detectDistance = 10f;
    public int damageGiven = 10;

    private bool notDetected;

    //For player interaction.
    public int startingHealth = 30; //Assuming a player's attack is 10 damage.
    int currentHealth;
    private bool moveToPlayer;
    private bool attackMode;

    // For enemy death and loot pickup.
    public AudioClip playerhitSFX;
    public AudioClip enemyHitSFX;

    public GameObject[] lootPrefabs;

    //For cube spin attack.
    private float degreeRotated;
    public float degreeToRotate;
    private bool hasNotRotated;
    public float maxDegreeRotation;
    Quaternion originalRotation;


    public static int cubeEnemyCount;

    void Start()
    {
        //Finds the player object.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        //Sets health.
        currentHealth = startingHealth;

        //Setting booleans.
        moveToPlayer = false;
        attackMode = false;
        hasNotRotated = true;
        notDetected = true;

        //Tracker for spin attack.
        degreeRotated = 0f;
        degreeToRotate = 0.01f;
        maxDegreeRotation = 2.7f;


        cubeEnemyCount += 1;

    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            //Determines if the player is close enough to be detected by the enemy.
            if (distance < detectDistance && notDetected)
            {
                transform.LookAt(player);
                moveToPlayer = true;
                notDetected = false;
            }

            //Moves the enemy closer to the enemy if detected.
            //Will not travel vertically towards the enemy.
            if (minDistance < distance && moveToPlayer)
            {
                attackMode = false;
                transform.position = Vector3.MoveTowards
                (transform.position,
                new Vector3(player.position.x, transform.position.y, player.position.z),
                step);
            }

            //Performs the spin attack every 3 seconds.
            if (distance <= minDistance)
            {
                attackMode = true;
                if (hasNotRotated)
                {
                    if (degreeRotated == 0){
                        originalRotation = transform.rotation;
                    }
                    if (degreeRotated < maxDegreeRotation){
                        //Debug.Log("Degree Rotated: " + degreeRotated);
                        degreeRotated += degreeToRotate;
                        transform.Rotate(Vector3.down, degreeRotated);  
                    }
                    else{
                        hasNotRotated = false;
                        transform.rotation = originalRotation;
                        Invoke("AttackAgain", 3f);
                    }
                }
            }
        }
        //Debug.Log(currentHealth);
    }

    //Resets variable for spin attack.
    void AttackAgain()
    {
        hasNotRotated = true;
        degreeRotated = 0f;
    }

    //Public class for player to call when they deal damage to the enemy.
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

    //Class that destroys the enemy and gives the pickup.
    void EnemyDies()
    {
        //AudioSource.PlayClipAtPoint(deadSFX, transform.position);

        LevelManager.enemyKillCount += 1;
        
        if (lootPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, lootPrefabs.Length);
            Instantiate(lootPrefabs[randomIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        Destroy(gameObject, 0.5f);
    }

   
    void OnTriggerEnter(Collider collision)
    {   
        if (collision.gameObject.CompareTag("Player") && attackMode)
        {
            PlayerControllerFixed player = collision.gameObject.GetComponent<PlayerControllerFixed>();
            player.TakeDamage(10);
            AudioSource.PlayClipAtPoint(playerhitSFX, transform.position);

        }
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            AudioSource.PlayClipAtPoint(enemyHitSFX, transform.position);
            EnemyAttacked(10);
        }
    }
}