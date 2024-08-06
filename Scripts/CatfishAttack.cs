using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatfishAttack : MonoBehaviour
{
    public int damageAmount = 5;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControllerFixed player = other.gameObject.GetComponent<PlayerControllerFixed>();
            player.TakeDamage(damageAmount);
        }
    }
    
}
