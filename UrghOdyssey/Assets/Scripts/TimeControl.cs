using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float hSliderValue = 1.0F;

    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(50, 50, 100, 30), hSliderValue, 0.0F, 10.0F);
    }

    void Update()
    {
        Time.timeScale = hSliderValue;
    }
}