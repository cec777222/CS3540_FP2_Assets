using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerFixed : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float gravity = 9.81f;
    public float airControl = 10;
    public static int playerHealth = 100;

    public AudioClip hitSFX;


    public int playerDamage = 10;

    CharacterController controller;
    Vector3 input, moveDirection;

    Renderer playerRender;
    GameObject weapon;
    public Slider healthslider;

    int damageRate;
    int currentHealth;

    bool isBlocking;
    private bool playerDead = false;

    private LevelManager levelManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        damageRate = 1;
        isBlocking = false;
        playerRender = GetComponent<Renderer>();
        weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");

        currentHealth = playerHealth;
        healthslider.value = currentHealth;
    }


    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= moveSpeed;

        if (controller.isGrounded)
        {
            moveDirection = input;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

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
            weapon.GetComponent<Animator>().SetTrigger("WeaponSwung");
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            if (isBlocking)
            {

            }
            else
            {
                currentHealth -= damage;
            }

            healthslider.value = currentHealth;
            Debug.Log(currentHealth);
            Debug.Log("hp" + healthslider.value);
        }
        if (currentHealth <= 0 && !playerDead)
        {
            PlayerDies();
            playerDead = true;
        }
    }

  

    void PlayerDies()
    {
        transform.Rotate(-90, 0, 0, Space.Self);
        levelManager.LevelLost();
    }
}

