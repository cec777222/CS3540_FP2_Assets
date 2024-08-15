using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowManBehavior : MonoBehaviour
{
    //For player detection and movement.
    public Transform player;
    public float moveSpeed = 1;
    public float minDistance = 1.5f;
    public float detectDistance = 15f;
    private bool notDetected;

    //For player interaction.
    public int startingHealth = 20; //Trying to get the player to kill it in 2 hits.
    int currentHealth;
    public Slider healthSlider;
    private bool moveToPlayer;
    static private bool aboutToBlow;

    // For enemy death and loot pickup.
    public AudioClip ExplosionSFX;
    public GameObject[] lootPrefabs;
    public GameObject IceExplosion;
    //public GameObject IceExplosionHitBox;

    void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        //Finds the player object.
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (IceExplosion == null)
        {
            IceExplosion = GameObject.FindGameObjectWithTag("Ice Explosion");
        }
        /*
        if (IceExplosionHitBox == null)
        {
            IceExplosion = GameObject.FindGameObjectWithTag("IceExplosionHitBox");
        }
        
        IceExplosionHitBox.SetActive(false);
        */
        //Sets health.
        currentHealth = startingHealth;
        //healthSlider.value = currentHealth;

        //Setting booleans.
        moveToPlayer = false;
        aboutToBlow = false;
        notDetected = true;

    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;

        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            //Determines if the player is close enough to be detected by the enemy.
            if (distance < detectDistance && notDetected && !aboutToBlow)
            {
                transform.LookAt(player);
                moveToPlayer = true;
                notDetected = false;
            }

            //Moves the enemy closer to the enemy if detected.
            //Will not travel vertically towards the enemy.
            if (minDistance < distance && moveToPlayer)
            {
                transform.position = Vector3.MoveTowards
                (transform.position,
                new Vector3(player.position.x, transform.position.y, player.position.z),
                step);
            }

            
            if (distance <= minDistance && !aboutToBlow)
            {
                moveToPlayer = false;
                aboutToBlow = true;
                StartExplosionWarning();
                Invoke("Explode", 8);
            }
        }
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(ExplosionSFX, transform.position);
        gameObject.SetActive(false);
        Instantiate(IceExplosion, transform.position, transform.rotation);
        //IceExplosionHitBox.SetActive(true);
        EnemyDies();
    }

    static public bool StartExplosionWarning()
    {
        return aboutToBlow;
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0){
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0){
            EnemyDies();
        }
    }

    //Class that destroys the enemy and gives the pickup.
    void EnemyDies()
    {
        Destroy(gameObject, 2f);
        //IceExplosion.SetActive(false);
        if (lootPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, lootPrefabs.Length);
            Instantiate(lootPrefabs[randomIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerWeaponAttackHitBox") || collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(10);
        }
    }
}