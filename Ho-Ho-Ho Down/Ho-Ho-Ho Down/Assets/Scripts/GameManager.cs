using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int presents = 0;
    bool snowing = true;
    bool gameOver = false;

    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    public void IncrementPresents()
    {
        presents++;
        FindObjectOfType<GameUIController>().UpdatePresentText(presents);
    }

    public int GetPresents()
    {
        return presents;
    }

    public void ResetGame()
    {
        presents = 0;
    }

    public bool GetSnowing()
    {
        return snowing;
    }

    public void SetSnowing(bool state)
    {
        snowing = state;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public void SetGameOver(bool state)
    {
        gameOver = state;
    }
}
