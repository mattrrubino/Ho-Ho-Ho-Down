using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI presentText;
    [SerializeField] TextMeshProUGUI presentTextScoreboard;
    [SerializeField] Image healthImage;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle snowToggle;
    [SerializeField] GameObject[] snowParticles;
    [SerializeField] Sprite[] healthSprites;
    Gun gun;
    Rigidbody2D player;
    PlayerMovement playerMovement;
    Music musicManager;
    SoundEffectManager sfxManager;
    Vector2 velocity;
    bool textLocked = false;
    bool snowing;
    GameManager gm;
    TransitionController tController;
    SoundEffectManager sfm;
    [SerializeField] bool mainMenu = false;
    [SerializeField] GameObject scoreboard;
    Rigidbody2D rb;
    [SerializeField] AudioClip click;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        musicManager = FindObjectOfType<Music>();
        sfm = FindObjectOfType<SoundEffectManager>();
        sfxManager = FindObjectOfType<SoundEffectManager>();
        tController = FindObjectOfType<TransitionController>();
        snowing = gm.GetSnowing();
        foreach(GameObject e in snowParticles)
        {
            e.SetActive(snowing);
        }

        if(snowToggle != null)
            snowToggle.isOn = snowing;

        if (!mainMenu)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            rb = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
            gun = FindObjectOfType<Gun>();
            player = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
            healthImage.sprite = healthSprites[5];
        }

        if(musicSlider != null)
        {
            sfxSlider.value = sfxManager.GetSFXVolume();
            musicSlider.value = musicManager.GetMusicVolume();
        }
    }

    public IEnumerator ManageScoreboard()
    {
        scoreboard.SetActive(true);

        yield return new WaitForSeconds(.1f);
        yield return new WaitUntil(() => Mathf.Abs(rb.velocity.magnitude) < .1f && Mathf.Abs(rb.angularVelocity) < .1f);
        FindObjectOfType<Scoreboard>().UpdateLeaderboard();
        yield return new WaitForSeconds(1f);

        presentTextScoreboard.text = presentText.text;
        Scoreboard sb = FindObjectOfType<Scoreboard>();
        if (sb.GetConnected())
            sb.CheckInitialInput();
        scoreboard.GetComponent<Animator>().SetTrigger("drop");
    }

    public void UpdatePresentText(int presents)
    {
        presentText.text = presents.ToString();
    }

    public void UpdateHealthText(int health)
    {
        if (!textLocked)
        {
            if(health > 0)
            {
                healthImage.sprite = healthSprites[health];
            }
            else
            {
                healthImage.sprite = healthSprites[0];
            }
        }
    }

    public void SetTextLocked(bool state)
    {
        textLocked = state;
    }

    public void Pause()
    {
        playerMovement.enabled = false;
        gun.UpdateInMenu(true);
        velocity = player.velocity;
        sfm.PlaySoundEffect(click);
        Debug.Log(velocity);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        sfm.PlaySoundEffect(click);
        StartCoroutine(ResumeCoroutine());
    }

    IEnumerator ResumeCoroutine()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        yield return new WaitForEndOfFrame();
        if (!gm.GetGameOver())
        {
            playerMovement.enabled = true;
            gun.UpdateInMenu(false);
            player.velocity = velocity;
        }
    }

    public void Options()
    {
        sfm.PlaySoundEffect(click);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        musicSlider.value = musicManager.GetMusicVolume();
        sfxSlider.value = sfxManager.GetSFXVolume();
        snowToggle.isOn = snowing;
    }

    public void OptionsBack()
    {
        sfm.PlaySoundEffect(click);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Menu()
    {
        sfm.PlaySoundEffect(click);
        tController.SetTransitionScene("Main Menu");
        gm.ResetGame();
    }

    public void PlayAgain()
    {
        sfm.PlaySoundEffect(click);
        tController.SetTransitionScene("Game");
        gm.ResetGame();
    }

    public void ToggleSound()
    {
        sfm.PlaySoundEffect(click);
    }

    void Update()
    {
        if (optionsMenu.activeSelf)
        {
            if(musicSlider != null)
            {
                musicManager.SetMusicVolume(musicSlider.value);
                sfxManager.SetSFXVolume(sfxSlider.value);
                snowing = snowToggle.isOn;
                gm.SetSnowing(snowing);
            }
            foreach (GameObject e in snowParticles)
            {
                e.SetActive(snowing);
            }
        }
    }
}
