using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseFunction : MonoBehaviour
{
    // BMo Pause menu

    private Player p;
    private AudioManager am;
    private LevelFade f;

    public GameObject PauseMenu;
    public static bool isPaused; // For ghosts on next level

    // Start is called before the first frame update
    void Start()
    {
        p = FindAnyObjectByType<Player>();
        am = FindAnyObjectByType<AudioManager>();
        f = FindAnyObjectByType<LevelFade>();
        PauseMenu.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void goToTitle()
    {
        am.Play("uiButton1");
        PauseMenu.SetActive(false);
        Time.timeScale = 0f;
        SceneManager.LoadScene("TitleCard");
        f.FadeToBlack(asyncLoadTitle);
        isPaused = false;
    }

    public void goToLevelSelectLevel1() {
        am.Play("uiButton1");
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        f.FadeToBlack(asyncLoadLevelSelect);
        isPaused = false;
    }

    public void PauseGame()
    {
        am.Play("Pause");
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        am.Play("Unpause");
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void asyncLoadLevelSelect()
    {
        StartCoroutine(changeScene("LevelSelect", new Vector3(0, -4, 0)));
    }

    public void asyncLoadTitle()
    {
        p.GetComponent<AudioListener>().enabled = false;
        StartCoroutine(changeScene("TitleCard", new Vector3(-10, 10, 0)));
    }

    public IEnumerator changeScene(string scene, Vector3 pos)
    {

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoadLevel.isDone)
            yield return null;

        p.transform.position = pos;
        p.receivingInput = true;
    }
}


