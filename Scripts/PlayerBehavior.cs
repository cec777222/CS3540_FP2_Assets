using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static int heartCount = 3;
    public float playerSpeed = 2f;
    public int playerDamage = 10;

    Rigidbody rb;
    int damageRate;
    bool isBlocking;
    Renderer playerRender;
    
    void Start()
    {
        damageRate = 1;
        rb = GetComponent<Rigidbody>();
        isBlocking = false;
        playerRender = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(horizontal, 0, vertical);
        rb.AddForce(moveVector * playerSpeed);

        if (Input.GetMouseButton(1))
        {
            isBlocking = true;
            
            playerRender.material.color = Color.blue;
        }
        else
        {
            isBlocking = false;
            playerRender.material.color = Color.white;
        }
        if (Input.GetMouseButton(0))
        {
            GameObject weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");

            weapon.GetComponent<Animator>().SetTrigger("WeaponSwung");

        }
    }

    public void TakeDamage(int damage)
    {
        if (!isBlocking)
        {
            damageRate = 1;
            
        }
        else
        {
            damageRate = 0;
        }
        heartCount -= (damage * damageRate);
    }

    void Attack()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //var enemy = GetComponent<CubeEnemyBehavior>();
            //enemy.EnemyAttacked(playerDamage);
        }
    }
}
