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
        }
    }

    private void Update()
    {
        if (enterAllowed && Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneToLoad);

            if (sceneToLoad == "LevelSelect")
            {
                this.transform.position = new Vector3(0,0,0);
            }
        }
    }
}
