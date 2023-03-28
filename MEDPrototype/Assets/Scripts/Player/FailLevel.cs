using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailLevel : MonoBehaviour
{   
    private Animator anim;

    private void start()
    {
        anim = GetComponent<Animator>();
        // SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) // Load level 1
        {

            Die();
            Debug.Log(collision.gameObject);
            Debug.Log("Hazard");
        } 
        else if (collision.GetComponent<DeathZone>())
        {
            anim = GetComponent<Animator>();
            Die();
            Debug.Log(collision.gameObject);
            // GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
            // this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
        }
    }

    private void Die()
    {
        anim.SetTrigger("Death");
    }

    private void restartLevel()
    {
        anim.ResetTrigger("Death");
        anim.SetTrigger("Idle");
        GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
        this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
    }
}
