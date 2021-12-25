using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfHitbox : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] AudioClip elfDeath;
    SoundEffectManager sfm;

    void Start()
    {
        sfm = FindObjectOfType<SoundEffectManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("ornament"))
        {
            Destroy(other.gameObject);
            Die();
        }
        else if(other.CompareTag("Player"))
        {
            if (other.GetComponent<SantaHealth>().GetDamageable())
            {
                other.GetComponent<SantaHealth>().DamageSanta(1, transform.position.x);
                Die();
            }
        }
    }

    void Die()
    {
        GameObject p = Instantiate(particles, transform.position, transform.rotation) as GameObject;
        p.transform.parent = GameObject.Find("Particles").transform;
        sfm.PlaySoundEffect(elfDeath);
        Destroy(transform.parent.gameObject);
    }
}
