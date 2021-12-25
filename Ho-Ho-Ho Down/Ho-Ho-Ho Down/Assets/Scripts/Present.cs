using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
    [SerializeField] GameObject presentParticle;
    [SerializeField] AudioClip presentPickup;
    [SerializeField] AudioClip healthPickup;
    SoundEffectManager sfm;
    int canSpawnIndex = 0;

    void Start()
    {
        sfm = FindObjectOfType<SoundEffectManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("health"))
            {
                CollectHealth();
            }
            else
                CollectPresent();
        }
    }

    private void CollectPresent()
    {
        FindObjectOfType<GameManager>().IncrementPresents();
        FindObjectOfType<RandomSpawner>().SetCanSpawn(canSpawnIndex, true);
        Effects(presentPickup);
    }

    private void CollectHealth()
    {
        FindObjectOfType<SantaHealth>().HealthPickup();
        FindObjectOfType<RandomSpawner>().SetCanSpawn(canSpawnIndex, true);
        Effects(healthPickup);
    }

    private void Effects(AudioClip clip)
    {
        GameObject particle = Instantiate(presentParticle, transform.position, transform.rotation);
        particle.GetComponentInChildren<SpriteRenderer>().sprite = 
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        particle.transform.localScale = gameObject.GetComponentInChildren<Transform>().localScale;
        particle.transform.SetParent(GameObject.Find("Particles").transform);
        sfm.PlaySoundEffect(clip);

        Destroy(particle, 0.5f);
        Destroy(gameObject);
    }

    public void SetCanSpawnIndex(int index)
    {
        canSpawnIndex = index;
    }
}
