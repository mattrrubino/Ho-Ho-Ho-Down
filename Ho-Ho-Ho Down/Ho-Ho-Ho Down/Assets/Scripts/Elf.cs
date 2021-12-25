using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elf : MonoBehaviour
{
    [Header("Casting")]
    [SerializeField] Transform feet;
    [SerializeField] float feetRange = .25f;
    [SerializeField] float yDiff = 1f;
    [SerializeField] float jumpCastRange = 2f;
    [SerializeField] float upCastRange = 30f;

    [Header("Tuning")]
    [SerializeField] float speed = 2f;
    [SerializeField] float jumpVelocity = 8f;
    [SerializeField] float atypicalMoveSetTime = .75f;
    [SerializeField] float stopRange = .1f;

    [SerializeField] bool canJump = true;
    [SerializeField] bool typicalMove = true;
    [SerializeField] bool facingRight = false;

    float atypicalMoveTime;
    bool castJustHit = false;
    bool circleJustHit = false;
    Rigidbody2D rb;
    Animator anim;
    Vector2 castOneDir = new Vector2(1.25f, 1f).normalized;
    Vector2 castTwoDir = new Vector2(-1.25f, 1f).normalized;
    Transform player;
    Vector2 direction;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        atypicalMoveTime = atypicalMoveSetTime;
    }

    private void DefaultDirections()
    {
        if (player.transform.position.x > transform.position.x)
            direction = Vector2.right;
        else
            direction = Vector2.left;
    }

    private void PlayerIsLower()
    {
        RaycastHit2D circleCast = Physics2D.CircleCast(feet.position, feetRange, Vector2.zero, 0f, 1 << 11);

        if (circleCast)
        {
            circleJustHit = true;
            FastDrop(circleCast);
        }
        else if (circleJustHit)
        {
            circleJustHit = false;
            atypicalMoveTime = .25f;
            StartCoroutine(AtypicalMove());
        }
        else
            DefaultDirections();
    }

    private void FastDrop(RaycastHit2D hit)
    {
        if (hit.transform.position.x < transform.position.x)
            direction = Vector2.right;
        else
            direction = Vector2.left;
    }

    private void PlayerIsHigher()
    {
        RaycastHit2D castUp = Physics2D.Raycast(transform.position, Vector2.up, upCastRange, 1 << 11);
        RaycastHit2D playerCheck = Physics2D.Raycast(transform.position, Vector2.up, upCastRange, 1 << 10);

        if (playerCheck)
        {
            if (castUp)
            {
                if (playerCheck.transform.position.y < castUp.transform.position.y)
                    direction = Vector2.zero;
            }
            else
                direction = Vector2.zero;
        }
        if (transform.position.x <= -3.5f)
        {
            if (transform.position.y < 4f)
                atypicalMoveTime = 3f;
            else
                atypicalMoveTime = 1.5f;

            direction = Vector2.right;
            StartCoroutine(AtypicalMove());
        }
        else if (transform.position.x >= 27.5f)
        {
            if (transform.position.y < 4f)
                atypicalMoveTime = 3f;
            else
                atypicalMoveTime = 1.5f;

            direction = Vector2.left;
            StartCoroutine(AtypicalMove());
        }
        else if (castUp)
        {
            EdgeCollider2D col = castUp.transform.gameObject.GetComponent<EdgeCollider2D>();
            var xOne = castUp.transform.position.x + col.points[0].x;
            var xTwo = castUp.transform.position.x + col.points[1].x;
            if (player.position.x >= xOne && player.position.x <= xTwo)
            {
                ShortestPath(castUp);
                castJustHit = true;
            }
            else
            {
                DefaultDirections();
                castJustHit = true;
            }
        }
        else if (transform.position.x <= 28.5f && transform.position.x >= -4.5f && castJustHit)
        {
            StartCoroutine(AtypicalMove());
            castJustHit = false;
        }
        else
            DefaultDirections();
    }

    private void ShortestPath(RaycastHit2D hit)
    {
        if (player.position.x > hit.transform.position.x)
            direction = Vector2.right;
        else
            direction = Vector2.left;
    }

    private void Update()
    {
        SpriteManagement();
        JumpReset();

        float heightDiff = player.position.y - transform.position.y;

        if (heightDiff <= yDiff / 10f && heightDiff > -yDiff)
        {
            if (Mathf.Abs(player.position.x - transform.position.x) < stopRange)
                return;
            else
                DefaultDirections();
        }
        else if (heightDiff <= -yDiff)
        {
            if(typicalMove)
                PlayerIsLower();
        }
        else if (!typicalMove)
        {
            if(canJump)
                CheckForJump();
        }
        else
        {
            PlayerIsHigher();

            if(canJump)
                CheckForJump();
        }
    }

    private void FixedUpdate()
    {
        if (transform.position.y - player.position.y <= yDiff && transform.position.y - player.position.y >= -yDiff / 10)
        {
            if (Mathf.Abs(transform.position.x - player.position.x) < stopRange)
                return;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void SpriteManagement()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) >= stopRange)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
            return;
        }

        if (direction == Vector2.left && facingRight)
            Flip();
        else if (direction == Vector2.right && !facingRight)
            Flip();
    }

    private void Flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        facingRight = !facingRight;
    }

    private void JumpReset()
    {
        if (Physics2D.OverlapCircle(feet.position, feetRange, 1 << 11) && rb.velocity.y == 0)
            canJump = true;
        else
            canJump = false;
    }

    private void CheckForJump()
    {
        RaycastHit2D castOne = Physics2D.Raycast(transform.position, castOneDir, jumpCastRange, 1 << 11);
        RaycastHit2D castTwo = Physics2D.Raycast(transform.position, castTwoDir, jumpCastRange, 1 << 11);

        Debug.DrawRay(feet.position, castOneDir, Color.red, Time.deltaTime);
        Debug.DrawRay(feet.position, castTwoDir, Color.red, Time.deltaTime);

        if (castOne && direction == Vector2.right)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
        else if (castTwo && direction == Vector2.left)
        {
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }

    private IEnumerator AtypicalMove()
    {
        typicalMove = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(atypicalMoveTime);
        atypicalMoveTime = atypicalMoveSetTime;
        typicalMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Left Wall")
        {
            direction = Vector2.right;
            atypicalMoveTime = 3f;
            StartCoroutine(AtypicalMove());
        }
        else if(other.gameObject.name == "Right Wall")
        {
            direction = Vector2.left;
            atypicalMoveTime = 3f;
            StartCoroutine(AtypicalMove());
        }
    }
}
