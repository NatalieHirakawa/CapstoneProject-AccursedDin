using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnterLevel : MonoBehaviour
{
    private bool enterAllowed;
    private string sceneToLoad;
#pragma warning disable
    private string currentScene;
#pragma warning restore
    private LevelFade f; //bad programming paradigm continued here due to time lack
    private AudioManager am;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        f = FindObjectOfType<LevelFade>();
        am = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Door")) 
        // {
            if (collision.GetComponent<Level1Door>()) // Load level 1
            {
                sceneToLoad = "Level1";
                currentScene = "LevelSelect";
                enterAllowed = true;
            }
            else if (collision.GetComponent<Level2Door>()) // Load level 2
            {
                sceneToLoad = "Level2";
                currentScene = "LevelSelect";
                enterAllowed = true;
            }
            else if (collision.GetComponent<Goal>())
            {
                sceneToLoad = "LevelSelect";
                enterAllowed = true;
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Level1Door>()) 
        // if (collision.GetComponent.tag == "LevelDoor")
        {
            enterAllowed = false;
        } else if (collision.GetComponent<Goal>())
        {
            enterAllowed = false;
        } else if (collision.GetComponent<Level2Door>())
        {
            enterAllowed = false;
        }
    }

    private void Update()
    {
        if (enterAllowed && Input.GetKey(KeyCode.E))
        {
            enterAllowed = false;
            am.Play("doorEnter");
            if (f != null)
                f.FadeToBlack(asyncLoad);
            else
                asyncLoad();
        }
    }

    public void asyncLoad()
    {
        StartCoroutine(changeScene());
    }

    public IEnumerator changeScene()
    {

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoadLevel.isDone)
            yield return null;

        if (sceneToLoad == "LevelSelect")
        {
            this.transform.position = new Vector3(-10, -1, 0);
        }
        else
        {
            GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
            if(startDoor != null)
                this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
        }

    }
}
