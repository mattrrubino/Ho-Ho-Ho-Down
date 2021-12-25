using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Ornament projPrefab;
    [SerializeField] float projSpeed = 100f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] Transform projStorage;
    [SerializeField] AudioClip gunSound;
    bool canShoot = true;
    bool inMenu = false;
    SoundEffectManager sManager;
    bool hoveredOverPause = false;

    void Start()
    {
        sManager = FindObjectOfType<SoundEffectManager>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && canShoot && !hoveredOverPause && !inMenu)
        {
            canShoot = false;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        Ornament proj = Instantiate(projPrefab, muzzle) as Ornament;
        proj.gameObject.transform.SetParent(projStorage);
        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(projSpeed * FindObjectOfType<PlayerMovement>().GetPlayerDirection(), 0f);
        sManager.PlaySoundEffect(gunSound);
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    public void UpdateHovered(bool state)
    {
        hoveredOverPause = state;
    }

    public void UpdateInMenu(bool state)
    {
        inMenu = state;
    }
}
