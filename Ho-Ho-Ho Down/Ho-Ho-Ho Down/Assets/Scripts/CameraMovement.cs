using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float xMin = 0f;
    [SerializeField] float xMax = 24f;
    [SerializeField] float yMin = 0f;
    [SerializeField] float yMax = 13.25f;
    Vector2 targetPos;
    Vector2 lerpedPos;
    Transform player;
    float distance;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void FixedUpdate()
    {
        targetPos = new Vector2(Mathf.Clamp(player.position.x, xMin, xMax), Mathf.Clamp(player.position.y, yMin, yMax));
        distance = Vector2.Distance(transform.position, targetPos);
        lerpedPos = Vector2.MoveTowards(transform.position, targetPos, speed * (distance + 0.5f) * Time.fixedDeltaTime);
        transform.position = new Vector3(lerpedPos.x, lerpedPos.y, -10f);
    }
}
