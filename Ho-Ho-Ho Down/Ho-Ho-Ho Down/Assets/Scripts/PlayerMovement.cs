using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Santa Stats")]
    [SerializeField] float speed = 4f;
    [SerializeField] float verticalVelocity = 10f;
    [SerializeField] float overlapRadius = .5f;
    [SerializeField] float fastFall = 1f;

    [Header("References")]
    [SerializeField] Transform feet;
    [SerializeField] LayerMask ground;
    Animator anim;
    Rigidbody2D rb;

    [Header("Tracking")]
    [SerializeField] bool isGrounded = true;
    [SerializeField] float delayedJump = .1f;
    bool wasGrounded = true;
    bool canJump = true;
    float xMovement, yMovement;
    bool facingRight = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement = Input.GetAxisRaw("Vertical");

        if (xMovement < 0 && facingRight)
            Flip();

        if (xMovement > 0 && !facingRight)
            Flip();

        anim.SetFloat("speed", Mathf.Abs(xMovement));

        if (rb.velocity.y <= 0)
            isGrounded = Physics2D.OverlapCircle(feet.position, overlapRadius, ground);

        if (isGrounded)
        {
            wasGrounded = true;
            canJump = true;
        }
        else if (wasGrounded)
        {
            wasGrounded = false;
            StartCoroutine(DelayedJump());
        }
    }

    IEnumerator DelayedJump()
    {
        yield return new WaitForSeconds(delayedJump);
        canJump = false;
    }

    private void FixedUpdate()
    {
        if (yMovement > 0 && canJump)
        {
            isGrounded = false;
            wasGrounded = false;
            canJump = false;
            Jump();
        }

        if (yMovement == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fastFall * Time.fixedDeltaTime);
        }

        if (yMovement < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fastFall * Time.fixedDeltaTime * 2f);
        }

        rb.velocity = new Vector2(xMovement * speed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void Flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        facingRight = !facingRight;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);
    }

    public int GetPlayerDirection()
    {
        if (facingRight)
            return 1;
        else
            return -1;
    }

    public bool GetGrounded()
    {
        return canJump;
    }
}
