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

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        p = GetComponent<Player>();
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
    }

    private void Die()
    {
        if (!dying)
        {
            p.audio.Play("die");
            dying = true;
            p.receivingInput = false;
            anim.SetTrigger("Death");
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
        GameObject startDoor = GameObject.FindGameObjectWithTag("Start");
        this.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
        anim.Play("Idle", -1, 0f);
        p.receivingInput = true;
        dying = false;
    }
}
