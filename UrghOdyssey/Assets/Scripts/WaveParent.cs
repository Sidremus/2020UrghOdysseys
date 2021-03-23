using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveParent : MonoBehaviour
{
    List<GameObject> listOfEnemiesInWave;
    WaveManager wm;
    private void Awake()
    {
        listOfEnemiesInWave = new List<GameObject>();
        wm = FindObjectOfType<WaveManager>();
        foreach(Transform child in transform)
        {
            listOfEnemiesInWave.Add(child.gameObject);
        }
        Debug.Log("This wave has " + listOfEnemiesInWave.Count + " enemies.");
    }
    public void EnemyHasBeenDestroyed(GameObject go)
    {
        if (listOfEnemiesInWave.Contains(go)) listOfEnemiesInWave.Remove(go);
        else Debug.LogError(go.name + " was not found in listOfEnemiesInWave");
        if (listOfEnemiesInWave.Count == 0) wm.SpawnNextWave();
    }
}
