using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Natalie Hirakawa
//Chris W

public class FailLevel : MonoBehaviour
{   
    private Animator anim;
    private Rigidbody2D rb;
    private Player p;
    private bool dying = false;
    [HideInInspector]
    public bool hasCheckpoint;
    private checkpoint checkpoint;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        p = GetComponent<Player>();
        hasCheckpoint = false;
    }

    /*private void Update() //this is trash bad aweful bad stupid temporary
    {
        if (dead)
        {
            timer += Time.deltaTime;
            if(timer > deathTime)
            {
                dead = false;
                restartLevel();
                timer = 0;
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) // Load level 1
        {
            Die();
            //Debug.Log(collision.gameObject);
            //Debug.Log("Hazard");
        } 
        else if (collision.GetComponent<DeathZone>())
        {
            Die();
            //Debug.Log(collision.gameObject);
            //GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
            //this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            hasCheckpoint = true;
        }
    }

    private void Die()
    {
        if (!dying)
        {
            p.audio.Play("die");
            dying = true;
            p.receivingInput = false;
            //anim.SetTrigger("playerDeath");
            anim.SetBool("isDead", dying);
        }
        //dead = true;
    }

    private void restartLevel()
    {
        //Debug.Log("restart!");
        LevelFade lf = FindAnyObjectByType<LevelFade>();
        if (lf != null)
            lf.FadeToBlack(asyncScene);
        else
            StartCoroutine(actuallyRestartLevel());
    }

    public void asyncScene()
    {
        StartCoroutine(actuallyRestartLevel());
    }

    public IEnumerator actuallyRestartLevel()
    {
        p.HaltVelocity();
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        while (!asyncLoadLevel.isDone)
            yield return null;

        //LevelFade lf = FindAnyObjectByType<LevelFade>();
        //if (lf != null)
        //    lf.forceFadeIn();
        if (!hasCheckpoint)
        {
            GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
            this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
        } else {
            checkpoint = FindObjectOfType<checkpoint>();  
            this.transform.position = checkpoint.gameObject.transform.position + new Vector3(0, 5, 0);
            //GameObject companion = GameObject.Instantiate(companions[2]) as GameObject;
            //companion.GetComponent<Companion>().companionIsFollowing = true;
            foreach (GameObject c in checkpoint.neededObjects)
            {
                c.transform.position = checkpoint.gameObject.transform.position + new Vector3(-1, -1, 0);
                c.GetComponent<Companion>().companionIsFollowing = true;
            }
        }
        anim.Play("playerIdle", -1, 0f);
        p.receivingInput = true;
        dying = false;
        anim.SetBool("isDead", dying);
    }
}
