using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseFunction : MonoBehaviour
{
    // BMo Pause menu

    public GameObject PauseMenu;
    public GameObject Player;
    public static bool isPaused; // For ghosts on next level

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
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
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleCard");
        Player.transform.position = new Vector3(-10,10,0);
        isPaused = false;
    }

    public void goToLevelSelectLevel1() {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
        Player.transform.position = new Vector3(0,-4,0);
        isPaused = false;
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}


