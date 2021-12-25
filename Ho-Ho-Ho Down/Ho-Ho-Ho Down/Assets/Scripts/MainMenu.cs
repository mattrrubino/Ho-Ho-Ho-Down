using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    TransitionController tController;
    Animator tAnimator;
    SoundEffectManager sfm;
    [SerializeField] AudioClip click;

    void Start()
    {
        tController = FindObjectOfType<TransitionController>();
        tAnimator = tController.GetComponent<Animator>();
        sfm = FindObjectOfType<SoundEffectManager>();
    }

    public void Play()
    {
        tController.SetTransitionScene("Game");
        sfm.PlaySoundEffect(click);
    }

    public void Instructions()
    {
        tController.SetTransitionScene("Instructions");
        sfm.PlaySoundEffect(click);
    }

    public void Options()
    {
        tController.SetTransitionScene("Options");
        sfm.PlaySoundEffect(click);
    }
}
