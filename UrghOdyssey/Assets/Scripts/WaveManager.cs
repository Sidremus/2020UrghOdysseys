using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyWaves;
    public int perfectWaveBonus = 10;
    public GameObject perfectWaveMessage;
    public ParticleSystem asteroids;
    public int waveToStartAsteroids;
    GameObject lastWave, currentWave;
    UIManager uiManager;
    int currentWaveNumber = 0, startOfWaveScore;
    bool currentlyResettingWave, nextWaveIsQueued, playerHasDiedOnCurrentWave;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        currentWave = Instantiate(enemyWaves[currentWaveNumber]);
        startOfWaveScore = GlobalStats.Highscore;
    }
    public void SpawnNextWave()
    {
        if (!currentlyResettingWave && !nextWaveIsQueued) StartCoroutine(NextWaveCoroutine());
    }
    private IEnumerator NextWaveCoroutine()
    {
        if (!playerHasDiedOnCurrentWave)
        {
            yield return new WaitForSeconds(1f);
            uiManager.IncreaseScoreCounter(perfectWaveBonus);
            if(GlobalStats.PlayerLives < 9) GlobalStats.PlayerLives++;
            uiManager.UpdateLifeCounter();
            perfectWaveMessage.SetActive(true);
            yield return new WaitForSeconds(2f);
            for (int n = 0; n < 7; n++)
            {
                perfectWaveMessage.SetActive(!perfectWaveMessage.activeSelf);
                yield return new WaitForSeconds(.3f);
            }
        }

        playerHasDiedOnCurrentWave = false;
        startOfWaveScore = GlobalStats.Highscore;

        nextWaveIsQueued = true;
        currentWaveNumber++;
        if (currentWaveNumber == waveToStartAsteroids && asteroids != null) asteroids.Play();

        if (enemyWaves.Length > currentWaveNumber)
        {
            lastWave = currentWave;
            Destroy(lastWave, 5f);
            currentWave = Instantiate(enemyWaves[currentWaveNumber]);
            print("now on wave " + currentWaveNumber);
        }
        else StartCoroutine(LoadVictoryScreen());
        StartCoroutine(AllowForNewWave());
        yield return null;
    }
    private IEnumerator AllowForNewWave()
    {
        yield return new WaitForSeconds(1f);
        nextWaveIsQueued = false;
    }
    private IEnumerator LoadVictoryScreen()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(4);
    }
    public void ResetCurrentWave()
    {
        StartCoroutine(ResetCoroutine());
    }
    private IEnumerator ResetCoroutine()
    {
        print("resetting wave " + currentWaveNumber);
        uiManager.SetScoreCounter(startOfWaveScore);
        playerHasDiedOnCurrentWave = true;
        currentlyResettingWave = true;
        lastWave = currentWave;
        Destroy(lastWave);
        yield return new WaitForSeconds(1f);
        currentWave = Instantiate(enemyWaves[currentWaveNumber]);
        currentlyResettingWave = false;
    }
    
}
