using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    //first sequence
    Animator anim;
    bool seq1 = true, seq2;
    int i;
    public TextMeshPro startButton;
    public ParticleSystem rocketEngine;
    public Vector3 launchedPos;
    public float textFlickerFreq = 3f;

    public AudioClip musicMainLoop;
    [Range(0f, 1f)] public float musicMainLoopVolume = 1f;
    AudioSource musicMainLoopSrc;
    
    public AudioClip musicIntroLoop;
    [Range(0f, 1f)] public float musicIntroLoopVolume = 1f;
    AudioSource musicIntroLoopSrc;

    public AudioClip confirmationSound;
    [Range(0f, 1f)] public float confirmationSoundVolume = 1f;
    AudioSource confirmationSoundSrc;

    //second sequence
    public GameObject tutorial;
    void Awake()
    {
        //musicIntroLoop 
        musicIntroLoopSrc = gameObject.AddComponent<AudioSource>();
        musicIntroLoopSrc.playOnAwake = false;
        musicIntroLoopSrc.volume = musicIntroLoopVolume;
        musicIntroLoopSrc.clip = musicIntroLoop;
        musicIntroLoopSrc.loop = true;
        //musicMainLoop 
        musicMainLoopSrc = gameObject.AddComponent<AudioSource>();
        musicMainLoopSrc.playOnAwake = false;
        musicMainLoopSrc.volume = musicMainLoopVolume;
        musicMainLoopSrc.clip = musicMainLoop;
        musicMainLoopSrc.loop = true;
        //musicMainLoop 
        confirmationSoundSrc = gameObject.AddComponent<AudioSource>();
        confirmationSoundSrc.playOnAwake = false;
        confirmationSoundSrc.volume = confirmationSoundVolume;
        confirmationSoundSrc.clip = confirmationSound;
        confirmationSoundSrc.loop = true;

        anim = GetComponent<Animator>();
        
    }
    private void Start()
    {
        musicMainLoopSrc.Play();
        StartCoroutine(AnimateTextColor());

        GlobalStats.Highscore = 0;
        GlobalStats.PlayerLives = 2;
        GlobalStats.PlayerDeaths = 0;
        GlobalStats.EnemiesKilled = 0;
    }
    private IEnumerator AnimateTextColor()
    {
        while (seq1)
        {
            startButton.color = Color.white;
            yield return new WaitForSeconds(1 / (textFlickerFreq * 2));
            startButton.color = Color.black;
            yield return new WaitForSeconds(1 / (textFlickerFreq * 2));
        }
        while (!seq1 && i < 50)
        {
            startButton.color = Color.white;
            yield return new WaitForSeconds(1 / (textFlickerFreq * 5));
            startButton.color = Color.black;
            yield return new WaitForSeconds(1 / (textFlickerFreq * 5));
            i++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.vKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
        {
            if (seq1) Launch();
            else if (seq2) CloseTutorial();
        }
        else if (Gamepad.current.added)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                if (seq1) Launch();
                else if (seq2) CloseTutorial();
            }
        }
    }

    private void CloseTutorial()
    {
        confirmationSoundSrc.PlayOneShot(confirmationSound, confirmationSoundVolume);
        SceneManager.LoadScene(2);
    }

    private void Launch()
    {
        seq1 = false;
        anim.SetTrigger("Launch");
        rocketEngine.Play();
        confirmationSoundSrc.PlayOneShot(confirmationSound, confirmationSoundVolume);
        Tween currentMove = transform.DOMove(launchedPos, 3f, false).SetEase(Ease.InBack);
        currentMove.OnComplete(ShowTutorial);
        void ShowTutorial()
        {
            StartCoroutine(TutorialSequence());
        }
    }
    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(2f);
        tutorial.SetActive(true);
        seq2 = true;
        musicMainLoopSrc.Stop();
        musicIntroLoopSrc.Play();
    }
}
