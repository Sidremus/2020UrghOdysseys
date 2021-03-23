using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public TextMeshPro highscore;
    public TextMeshPro startButton;
    bool seq1 = true;
    AudioSource oneShotter;
    public AudioClip gameOverSound;
    [Range(0f, 1f)] public float gameOverSoundVolume = 1f;

    public AudioClip confirmationSound;
    [Range(0f, 1f)] public float confirmationSoundVolume = 1f;


    public CustomEvent _HS;

    private void Awake()
    {
        oneShotter = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        highscore.text = GlobalStats.Highscore.ToString();
        oneShotter.PlayOneShot(gameOverSound, gameOverSoundVolume);
        StartCoroutine(AnimateTextColor());
        
    }


    private IEnumerator AnimateTextColor()
    {
        while (seq1)
        {
            startButton.color = Color.white;
            yield return new WaitForSeconds(.25f);
            startButton.color = Color.black;
            yield return new WaitForSeconds(.25f);
        }

        while (!seq1)
        {
            startButton.color = Color.white;
            yield return new WaitForSeconds(.125f);
            startButton.color = Color.black;
            yield return new WaitForSeconds(.125f);
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
        {
            seq1 = false;
            oneShotter.PlayOneShot(confirmationSound, confirmationSoundVolume);
            StartCoroutine(LoadNextScene());
        }
        else if (Gamepad.current.added)
        {
            if(Gamepad.current.buttonEast.wasPressedThisFrame || Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                seq1 = false;
                oneShotter.PlayOneShot(confirmationSound, confirmationSoundVolume);
                StartCoroutine(LoadNextScene());
            }
        }
    }
}