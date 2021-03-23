using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStats
{
    public static int Highscore { get; set; } = 2;
    public static int PlayerDeaths { get; set; } = 0;
    public static int EnemiesKilled { get; set; } = 0;
    public static int PlayerLives { get; set; } = 0;
}
