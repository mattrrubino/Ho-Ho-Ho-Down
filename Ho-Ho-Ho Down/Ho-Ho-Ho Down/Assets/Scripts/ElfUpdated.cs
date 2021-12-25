using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfUpdated : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] float yBelow = .5f;
    [SerializeField] float yAbove = 2f;
    [SerializeField] float xProximity = .1f;
    [SerializeField] float feetRange = .25f;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpXDist = .1f;
    [SerializeField] float jumpForce = 8f;

    [Header("References")]
    [SerializeField] Transform feet;
    [SerializeField] GameObject platObj;
    [SerializeField] Vector2[] platforms;

    // Cached variables
    Transform player;
    Rigidbody2D rb;
    Vector2 direction = Vector2.zero;
    [SerializeField] bool atypicalMove = false;
    [SerializeField] bool canJump = false;
    bool footCastHit = false;
    Transform lastRaycast;
    Transform[] plats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>().transform;
        plats = platObj.GetComponentsInChildren<Transform>();
        platforms = new Vector2[plats.Length];
        for (int i = 0; i < plats.Length; i++)
            platforms[i] = plats[i].position;
    }

    void Update()
    {
        ElfMovement();
    }

    private void ElfMovement()
    {
        if (!atypicalMove)
        {
            if (player.position.y > transform.position.y + yAbove)
                SantaAbove();
            else if (player.position.y < transform.position.y - yBelow)
                SantaBelow();
            else
                GetDefaultDirections();
        }
    }

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void GetDefaultDirections()
    {
        if (player.position.x > transform.position.x + xProximity)
            direction = Vector2.right;
        else if (player.position.x < transform.position.x - xProximity)
            direction = Vector2.left;
        else
            direction = Vector2.zero;
    }

    private void SantaBelow()
    {
        RaycastHit2D rayCast = Physics2D.Raycast(feet.position, Vector2.down, 50f, 1 << 11);

        if (lastRaycast)
        {
            if(!(rayCast.transform.gameObject.name.Equals(lastRaycast.transform.gameObject.name)) && !atypicalMove)
            {
                StartCoroutine(FallAtypical());
                return;
            }
        }

        lastRaycast = rayCast.transform;
        EdgeCollider2D col = rayCast.transform.gameObject.GetComponent<EdgeCollider2D>();
        float leftX = rayCast.transform.position.x + col.points[0].x;
        float rightX = rayCast.transform.position.x + col.points[1].x;

        if (player.position.x <= rightX && player.position.x >= leftX)
        {
            if (transform.position.x > rayCast.transform.position.x)
                direction = Vector2.right;
            else
                direction = Vector2.left;
        }
        else
            GetDefaultDirections();
    }

    private void SantaAbove()
    {
        ArrayList viablePlatforms = new ArrayList();

        foreach (Vector2 p in platforms)
        {
            if (p.y > transform.position.y && p.y - transform.position.y < 6f)
                viablePlatforms.Add(p);
        }

        RaycastHit2D playerCast = Physics2D.Raycast(player.position, Vector2.down, 50f, 1 << 11);
        Vector2 targetPlatform = Vector2.zero;
        float distance = 1000f;
        foreach(Vector2 p in viablePlatforms)
        {
            if(Vector2.Distance(p, transform.position) + Vector2.Distance(playerCast.transform.position, p) < distance)
            {
                targetPlatform = p;
                distance = Vector2.Distance(p, transform.position) + Vector2.Distance(playerCast.transform.position, p);
            }
        }

        if (targetPlatform.x > transform.position.x)
            direction = Vector2.right;
        else
            direction = Vector2.left;

        CheckForJump(targetPlatform);
    }

    private void CheckForJump(Vector2 targetPlat)
    {
        if (Physics2D.OverlapCircle(feet.position, feetRange) && rb.velocity.y == 0)
            canJump = true;

        if (!canJump)
            return;

        RaycastHit2D circleCast = Physics2D.CircleCast(feet.position, feetRange, Vector2.zero, 0f, 1 << 11);
        if (circleCast.transform.gameObject.CompareTag("elfground"))
        {
            Transform tPlat = feet;
            foreach(Transform p in plats)
            {
                if ((Vector2)p.position == targetPlat)
                    tPlat = p;
            }
            EdgeCollider2D col = tPlat.gameObject.GetComponent<EdgeCollider2D>();
            float leftX = tPlat.position.x + col.points[0].x;
            float rightX = tPlat.position.x + col.points[1].x;

            if (leftX - transform.position.x < jumpXDist && transform.position.x - rightX < jumpXDist)
                Jump();
        }
        else
        {
            EdgeCollider2D col = circleCast.transform.gameObject.GetComponent<EdgeCollider2D>();
            float leftX = circleCast.transform.position.x + col.points[0].x;
            float rightX = circleCast.transform.position.x + col.points[1].x;

            if (leftX - transform.position.x < jumpXDist && transform.position.x - rightX < jumpXDist)
                Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJump = false;
        StartCoroutine(JumpAtypical());
    }

    private IEnumerator JumpAtypical()
    {
        atypicalMove = true;
        yield return new WaitForSeconds(.1f);
        yield return new WaitUntil(() => Physics2D.OverlapCircle(feet.position, feetRange) && rb.velocity.y == 0);
        atypicalMove = false;
    }

    private IEnumerator FallAtypical()
    {
        atypicalMove = true;
        yield return new WaitForSeconds(.1f);
        atypicalMove = false;
    }
}
