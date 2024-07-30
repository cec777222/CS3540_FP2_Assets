using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    public int damageGiven = 10;
    public AudioClip playerhitSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {   
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControllerFixed player = collision.gameObject.GetComponent<PlayerControllerFixed>();
            player.TakeDamage(damageGiven);
            //AudioSource.PlayClipAtPoint(playerhitSFX, transform.position);

        }
    }
}
