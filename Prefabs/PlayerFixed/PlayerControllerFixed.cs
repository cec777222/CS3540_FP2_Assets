using System;
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

    //Renderer playerRender;
    GameObject weapon;
    public Slider healthslider;

    int damageRate;
    int currentHealth;

    bool isBlocking;
    private bool playerDead = false;

    Animator playerAnim;

    private LevelManager levelManager;
    public GameObject projectilePrefab;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        if (controller.velocity != Vector3.zero/*moveHorizontal != 0 || moveVertical != 0*/)
        {
            var c_x = controller.velocity.x;
            var c_z = controller.velocity.z;
            playerAnim.SetInteger("animState", 1);

            if (c_z < 0)
            {
                if (MathF.Abs(c_z) > MathF.Abs(c_x))
                {
                    SetWalkAnim("forward");
                }
                else
                {
                    if (c_x < 0)
                    {
                        SetWalkAnim("right");
                    }
                    else
                    {
                        SetWalkAnim("left");
                    }
                }
            }
            else
            {
                if (MathF.Abs(c_z) > MathF.Abs(c_x))
                {
                    SetWalkAnim("back");
                }
                else
                {
                    if (c_x < 0)
                    {
                        SetWalkAnim("right");
                    }
                    else
                    {
                        SetWalkAnim("left");
                    }
                }
            }
            
           
        }
        if(controller.velocity == Vector3.zero ) 
        {
            NoWalking();
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
            Debug.Log(currentHealth);
            Debug.Log("hp" + healthslider.value);
        }
        if (currentHealth <= 0 && !playerDead)
        {
            PlayerDies();
            playerDead = true;
            playerAnim.SetInteger("animState", 5);
            playerAnim.SetTrigger("isDead");
            
        }
    }

  

    void PlayerDies()
    {
        //transform.Rotate(-90, 0, 0, Space.Self);
        playerAnim.SetInteger("animState", 5);
        //this line doesnt work because it will set the state to 5 then switch immediately back to 0
        //levelManager.LevelLost();
    }

    void Shoot()
    {
        LevelManager lm = FindObjectOfType<LevelManager>();
        if (lm.playerCanShoot)
        {
            GameObject projectile;
            projectile = Instantiate(projectilePrefab, transform.position , transform.rotation);
            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("ProjectileParent").transform);
        }
        
    }

    void NoWalking()
    {
        playerAnim.SetBool("forward", false);
        playerAnim.SetBool("left", false);
        playerAnim.SetBool("right", false);
        playerAnim.SetBool("back", false);

    }

    void SetWalkAnim(string setParam)
    {
        foreach (AnimatorControllerParameter parameter in playerAnim.parameters) {
            if(parameter.type == AnimatorControllerParameterType.Bool)
            {
                if (parameter.name == setParam )
                            {
                                playerAnim.SetBool(parameter.name, true);
                            }
                            else
                            {
                                playerAnim.SetBool(parameter.name, false);
                            }
            }
            
        }
    }

}

