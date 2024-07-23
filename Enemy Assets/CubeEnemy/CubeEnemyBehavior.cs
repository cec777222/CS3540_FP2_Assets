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
    private bool notDetected;

    //For player interaction.
    public int startingHealth = 30; //Assuming a player's attack is 10 damage.
    int currentHealth;
    public AudioClip deadSFX;
    private bool moveToPlayer;
    private bool attackMode;

    //For cube spin attack.
    public float duration = 2f; // Duration of the rotation
    private bool hasNotRotated;
    float elapsedTime;
    public int damageGiven;

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
        hasNotRotated = true;
        notDetected = true;

        //Time tracker for spin attack.
        elapsedTime = 0f;
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
                //gameObject.GetComponent<Animator>().SetTrigger("PlayerDetected");
                transform.position = Vector3.MoveTowards
                (transform.position,
                new Vector3(player.position.x, transform.position.y, player.position.z),
                step);
            }

            //Preforms the spin attack every 3 seconds.
            if (distance <= minDistance)
            {
                attackMode = true;
                //gameObject.GetComponent<Animator>().SetTrigger("AttackDistance");
                //Quaternion originalRotation = transform.rotation;
                if (elapsedTime < duration && hasNotRotated)
                {
                    transform.Rotate(Vector3.down, 360 * Time.deltaTime * 0.5f);
                    elapsedTime += Time.deltaTime;
                    //Debug.Log("Elapsed time: " + elapsedTime + ". Duration: " + duration);
                }
                if (elapsedTime > duration)
                {
                    Debug.Log("Elapsed time: " + elapsedTime + ". Duration: " + duration);
                    //transform.rotation = originalRotation;
                    //transform.LookAt(player);
                    attackMode = false;
                    hasNotRotated = false;
                    Invoke("AttackAgain", 3f);
                }

            }
        }
        Debug.Log(currentHealth);
    }

    //Resets variable for spin attack.
    void AttackAgain()
    {
        elapsedTime = 0f;
        hasNotRotated = true;
    }


    //Public class for for player to call when they deal damage to the enemy.
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
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
        //Instantiate(prefab);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackMode)
        {
            damageGiven = 10;
            var playerHealth = collision.gameObject.GetComponent<PlayerBehavior>();
            playerHealth.TakeDamage(damageGiven);
            
        }
    }

   
}