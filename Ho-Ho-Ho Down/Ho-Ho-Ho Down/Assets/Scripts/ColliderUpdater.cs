using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderUpdater : MonoBehaviour
{
    [SerializeField] PlatformEffector2D[] effectors;
    [SerializeField] float fallTime = 0.5f;
    PlayerMovement player;
    Rigidbody2D rb;
    bool collidersActive = true;
    float yMovement;
    bool activeCoroutine = false;
    bool effectorsUp = true;
    Coroutine fall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        yMovement = Input.GetAxisRaw("Vertical");

        if(player.GetGrounded() && yMovement < 0  && collidersActive && !activeCoroutine)
        {
            activeCoroutine = true;
            fall = StartCoroutine(Fall());
        }

        if(activeCoroutine && yMovement != -1)
        {
            activeCoroutine = false;
            StopCoroutine(fall);
        }

        if(yMovement != -1 && !effectorsUp)
        {
            FlipEffectorsUp();
            effectorsUp = true;
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallTime);
        FlipEffectorsDown();
        effectorsUp = false;
    }

    private void FlipEffectorsDown()
    {
        foreach (PlatformEffector2D effector in effectors)
        {
            effector.rotationalOffset = -180f;
        }
        collidersActive = false;
    }

    private void FlipEffectorsUp()
    {
        foreach (PlatformEffector2D effector in effectors)
        {
            effector.rotationalOffset = 0f;
        }
        collidersActive = true;
    }
}
