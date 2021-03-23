using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAsteroids : MonoBehaviour
{
    GameObject asteroids;
    private void Awake()
    {
        asteroids = GameObject.FindGameObjectWithTag("Asteroids");
        Debug.Log(asteroids.name);
    }
    private void OnDestroy()
    {
        if (asteroids != null) 
        {
            asteroids.SetActive(true);
        }
    }
}
