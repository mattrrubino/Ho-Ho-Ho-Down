using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField] GameObject transitionCanvas;
    string targetScene = "";
    Animator anim;

    void Awake()
    {
        if(FindObjectsOfType<TransitionController>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        transitionCanvas.SetActive(false);
        anim = GetComponent<Animator>();
    }

    public void Transition()
    {
        if (targetScene != null)
        {
            SceneManager.LoadScene(targetScene);
            Time.timeScale = 1f;
        }
        else
            Debug.LogError("No target scene");
    }

    public void End()
    {
        transitionCanvas.SetActive(false);
        anim.SetTrigger("end");
    }

    public void SetTransitionScene(string scene)
    {
        targetScene = scene;
        transitionCanvas.SetActive(true);
        anim.SetTrigger("transition");
    }
}
