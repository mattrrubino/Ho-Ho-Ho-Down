using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField] float soundFXVolume = 0.5f;
    AudioSource aSource;

    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        aSource.PlayOneShot(clip, soundFXVolume);
    }

    public float GetSFXVolume()
    {
        return soundFXVolume;
    }

    public void SetSFXVolume(float volume)
    {
        soundFXVolume = volume;
    }
}
