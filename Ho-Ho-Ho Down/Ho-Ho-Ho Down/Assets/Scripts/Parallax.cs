using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float multiplier = .25f;
    Vector2 startPos;
    Transform cam;
    Vector2 movedPos;

    void Start()
    {
        startPos = transform.position;
        cam = Camera.main.transform;
    }

    void Update()
    {
        movedPos = new Vector2(startPos.x + (cam.position.x * multiplier), startPos.y + (cam.position.y * multiplier));
        transform.position = movedPos;
    }
}
