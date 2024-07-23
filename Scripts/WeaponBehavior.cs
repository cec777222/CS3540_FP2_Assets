using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Chop()
    {
        //GetComponent<Animator>().SetTrigger("WeaponSwung");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        if (other.gameObject.CompareTag("Enemy"))
        {
            var player = GetComponent<PlayerBehavior>();
            player.Attack(other);

        }
    }
}
