using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    public SpriteRenderer blackScreen;
    public Color col1, col2, col3, col4;
    public float shortTime = .25f, longTime = 3f;
    void Start()
    {
        blackScreen.color = col1;
        StartCoroutine(IntroCutsceneCoroutine());
    }
    private IEnumerator IntroCutsceneCoroutine()
    {
        yield return new WaitForSeconds(shortTime);
        blackScreen.color = col2;
        yield return new WaitForSeconds(shortTime);
        blackScreen.color = col3;
        yield return new WaitForSeconds(shortTime);
        blackScreen.color = col4;
        yield return new WaitForSeconds(longTime);
        blackScreen.color = col3;
        yield return new WaitForSeconds(shortTime);
        blackScreen.color = col2;
        yield return new WaitForSeconds(shortTime);
        blackScreen.color = col1;
        yield return new WaitForSeconds(shortTime);
        SceneManager.LoadScene(1);
    }
}
