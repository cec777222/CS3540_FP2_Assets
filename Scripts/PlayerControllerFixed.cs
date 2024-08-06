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
    public float jumpHeight = 2f;

    public AudioClip hitSFX;

    public int playerDamage = 10;

    CharacterController controller;
    Vector3 input, moveDirection;

    //Renderer playerRender;
    GameObject weapon;
    public Slider healthslider;
    int currentHealth;

    int damageRate;


    bool isBlocking;
    private bool playerDead = false;

    Animator playerAnim;

    LevelManager levelManager;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        damageRate = 1;
        isBlocking = false;
        //playerRender = GetComponent<Renderer>();
        weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        playerAnim = GetComponent<Animator>();

        currentHealth = playerHealth;
        healthslider.value = currentHealth;

        levelManager = FindObjectOfType<LevelManager>();
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

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }

        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);

        if (controller.velocity.z < 0/*moveHorizontal != 0 || moveVertical != 0*/)
        {
            playerAnim.SetInteger("animState", 1);
        }
        if (controller.velocity == Vector3.zero)
        {
            playerAnim.SetInteger("animState", 0);
        }
        controller.Move(moveDirection * Time.deltaTime);
        if (Input.GetMouseButton(1))
        {
            isBlocking = true;
            playerAnim.SetInteger("animState", 4);
            //playerRender.material.color = Color.blue;
        }
        else
        {
            isBlocking = false;
            //playerRender.material.color = Color.white;
        }

        if (Input.GetMouseButton(0))
        {
            //HOLD DOWN THE BUTTON or you will not see the animation :)
            playerAnim.SetInteger("animState", 3);
            //weapon.GetComponent<Animator>().SetTrigger("WeaponSwung");
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
        }
        if (currentHealth <= 0 && !playerDead)
        {
            PlayerDies();
            playerDead = true;

        }
    }

    void PlayerDies()
    {
        levelManager.LevelLost();
        playerAnim.SetInteger("animState", 5);
    }
}