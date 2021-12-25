using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SantaHealth : MonoBehaviour
{
    bool damageable = true;

    [Header("Health")]
    [SerializeField] int maxHealth = 5;
    [SerializeField] int currentHealth;

    [Header("Timing")]
    [SerializeField] float waitTime = 1f;
    [SerializeField] float flickerTime = 1f;
    [SerializeField] int numberOfFlickers = 3;

    [Header("Death")]
    [SerializeField] float deathTorque = -10f;
    [SerializeField] float stationaryTorque = 30f;
    [SerializeField] PhysicsMaterial2D pm;

    // References
    Rigidbody2D rb;
    GameUIController ui;
    Animator anim;
    GameManager gm;
    [SerializeField] SpriteRenderer rend;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ui = FindObjectOfType<GameUIController>();
        currentHealth = maxHealth;
        ui.UpdateHealthText(currentHealth);
        gm = FindObjectOfType<GameManager>();
        gm.SetGameOver(false);
    }

    public void DamageSanta(int damage, float elfX)
    {
        currentHealth -= damage;
        ui.UpdateHealthText(currentHealth);

        if (currentHealth <= 0)
            GameOver(elfX);
        else
            StartCoroutine(Flicker());
    }

    void GameOver(float elfX)
    {
        ui.UpdateHealthText(0);
        ui.SetTextLocked(true);
        ui.StartCoroutine(ui.ManageScoreboard());
        gm.SetGameOver(true);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Gun>().enabled = false;
        FindObjectOfType<RandomSpawner>().StopGame();
        anim.SetFloat("speed", 0f);
        anim.SetTrigger("death");

        foreach (Elf e in FindObjectsOfType<Elf>())
        {
            e.GetComponent<Animator>().SetBool("walking", false);
            e.enabled = false;
        }

        foreach(EdgeCollider2D col in FindObjectsOfType<EdgeCollider2D>())
        {
            col.sharedMaterial = pm;
        }

        rb.constraints = RigidbodyConstraints2D.None;

        if(rb.velocity.x == 0)
        {
            if (elfX < transform.position.x)
                rb.AddTorque(-stationaryTorque);
            else
                rb.AddTorque(stationaryTorque);
        }
        else
            rb.AddTorque(rb.velocity.x * deathTorque);
    }

    IEnumerator Flicker()
    {
        damageable = false;
        float flickerStore = flickerTime / numberOfFlickers / 2;
        for(int i = 0; i < numberOfFlickers; i++)
        {
            rend.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(flickerStore);
            rend.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flickerStore);
        }
        damageable = true;
    }

    public bool GetDamageable()
    {
        return damageable;
    }

    public void HealthPickup()
    {
        if(currentHealth < maxHealth && !gm.GetGameOver())
        {
            currentHealth++;
            ui.UpdateHealthText(currentHealth);
        }
    }
}
