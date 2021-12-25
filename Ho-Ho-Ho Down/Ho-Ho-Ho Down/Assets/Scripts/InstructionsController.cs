using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsController : MonoBehaviour
{
    [SerializeField] AudioClip click;

    public void Back()
    {
        FindObjectOfType<TransitionController>().SetTransitionScene("Main Menu");
        FindObjectOfType<SoundEffectManager>().PlaySoundEffect(click);
    }
}
