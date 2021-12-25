using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource aSource;
    float musicVolume;

    void Awake()
    {
        if (FindObjectsOfType<Music>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
        musicVolume = aSource.volume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        aSource.volume = musicVolume;
    }
}
