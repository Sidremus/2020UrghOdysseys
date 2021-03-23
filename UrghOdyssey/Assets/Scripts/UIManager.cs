using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshPro scoreCounter;
    public TextMeshPro lifeCounter;
    void Start()
    {
        scoreCounter.text = GlobalStats.Highscore.ToString();
        UpdateLifeCounter();
    }
    public void IncreaseScoreCounter(int value)
    {
        GlobalStats.Highscore += value;
        scoreCounter.text = GlobalStats.Highscore.ToString();
    }
    public void SetScoreCounter(int value)
    {
        GlobalStats.Highscore = value;
        scoreCounter.text = GlobalStats.Highscore.ToString();
    }
    public void UpdateLifeCounter()
    {
        lifeCounter.text = GlobalStats.PlayerLives.ToString();
    }
}
