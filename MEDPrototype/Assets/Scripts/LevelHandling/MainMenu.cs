using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Player p;
    private AudioManager am;
    private LevelFade f;

    public void Awake()
    {
        p = FindFirstObjectByType<Player>();
        am = FindFirstObjectByType<AudioManager>();
        f = FindFirstObjectByType<LevelFade>();
        if (p != null) {p.receivingInput = false; p.HaltVelocity(); }
    }

    public void StartGame()
    {
        am.Play("uiButton1");
        f.FadeToBlack(asyncLoad);
        if (p != null)
            p.receivingInput = true;
    }

    public void QuitGame()
    {
        am.Play("uiButton2");
        Application.Quit();
    }

    public void asyncLoad()
    {
        StartCoroutine(changeScene());
    }

    public IEnumerator changeScene()
    {

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("LevelSelect");

        while (!asyncLoadLevel.isDone)
            yield return null;

    }
}
