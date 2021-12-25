using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ornament : MonoBehaviour
{
    [SerializeField] float speed = -30f;
    int colorIndex;
    Color[] colors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green,
        Color.magenta, Color.red, Color.yellow, Color.white };
    float zRot;

    void Start()
    {
        colorIndex = Random.Range(0, 8);
        GetComponent<SpriteRenderer>().color = colors[colorIndex];
    }

    void Update()
    {
        zRot += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zRot * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("boundary"))
            StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
