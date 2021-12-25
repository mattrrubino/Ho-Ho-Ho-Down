using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SpriteRenderer[] platforms;
    [SerializeField] GameObject[] presents;
    [SerializeField] GameObject elf;

    [Header("Tuning")]
    [SerializeField] float spawnOffset = 0.5f;
    [SerializeField] float presentHeightOffset = 0.25f;
    [SerializeField] float elfHeightOffset = 0f;
    [SerializeField] float presentSpawnInterval = 3f;

    [Header("Variables")]
    [SerializeField] bool[] canSpawn;
    float timer = 0;
    Coroutine elfCoroutine;
    Coroutine presentCoroutine;

    void Update()
    {
        timer += Time.deltaTime;
    }

    void Start()
    {
        canSpawn = new bool[platforms.Length];

        for (int i = 0; i < canSpawn.Length; i++)
            canSpawn[i] = true;

        elfCoroutine = StartCoroutine(SpawnElves());
        presentCoroutine = StartCoroutine(SpawnPresents());
    }

    IEnumerator SpawnPresents()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (Random.Range(0, 12) == 0)
                RandomSpawn(presents[6]);
            else
                RandomSpawn(presents[Random.Range(0, presents.Length - 1)]);

            yield return new WaitForSeconds(presentSpawnInterval);
        }
    }

    public void RandomSpawn(GameObject obj)
    {
        List<GameObject> spawnable = new List<GameObject>();

        if (obj.GetComponent<Present>())
        {
            for(int i = 0; i < platforms.Length; i++)
            {
                if(!platforms[i].isVisible && canSpawn[i])
                {
                    spawnable.Add(platforms[i].gameObject);
                }
            }

            if(spawnable.Count > 0)
            {
                int platIndex = Random.Range(0, spawnable.Count - 1);
                GameObject plat = spawnable[platIndex];

                int canSpawnIndex = 0;

                for (int i = 0; i < platforms.Length; i++)
                {
                    if (platforms[i].gameObject == plat)
                        canSpawnIndex = i;
                }

                canSpawn[canSpawnIndex] = false;

                float platLength = Mathf.Abs(plat.GetComponent<EdgeCollider2D>().points[0].x -
                    plat.GetComponent<EdgeCollider2D>().points[1].x);
                float xLocation = Random.Range(plat.transform.position.x - (platLength / 2) + spawnOffset,
                    plat.transform.position.x + (platLength / 2) - spawnOffset);

                GameObject present = Instantiate(obj, gameObject.transform) as GameObject;
                present.transform.position = new Vector2(xLocation, plat.transform.position.y + plat.GetComponent<EdgeCollider2D>().offset.y + presentHeightOffset);
                present.GetComponent<Present>().SetCanSpawnIndex(canSpawnIndex);
            }
        }
        else
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                if (!platforms[i].isVisible)
                {
                    spawnable.Add(platforms[i].gameObject);
                }
            }

            if (spawnable.Count > 0)
            {
                int platIndex = Random.Range(0, spawnable.Count - 1);
                GameObject plat = spawnable[platIndex];

                float platLength = Mathf.Abs(plat.GetComponent<EdgeCollider2D>().points[0].x -
                    plat.GetComponent<EdgeCollider2D>().points[1].x);
                float xLocation = Random.Range(plat.transform.position.x - (platLength / 2) + spawnOffset,
                    plat.transform.position.x + (platLength / 2) - spawnOffset);

                GameObject elf = Instantiate(obj, gameObject.transform) as GameObject;
                elf.transform.position = new Vector2(xLocation, plat.transform.position.y + plat.GetComponent<EdgeCollider2D>().offset.y + elfHeightOffset);
            }
        }
    }

    IEnumerator SpawnElves()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            RandomSpawn(elf);
            yield return new WaitForSeconds(CalcWait(timer));
        }
    }

    private float CalcWait(float time)
    {
        return (2 / ((1 / 128) * time + 1) + 1);
    }

    public void SetCanSpawn(int index, bool state)
    {
        canSpawn[index] = state;
    }

    public void StopGame()
    {
        StopCoroutine(elfCoroutine);
        StopCoroutine(presentCoroutine);
        enabled = false;
    }
}
