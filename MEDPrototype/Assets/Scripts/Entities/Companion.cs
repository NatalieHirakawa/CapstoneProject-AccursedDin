using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W
//Ai Le

public class Companion : MonoBehaviour
{
    [SerializeField] private Player player;
    private AudioManager am;
    ///private SpriteRenderer playerRenderer;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private SpriteRenderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    
    [SerializeField] private float movePercent = 0.75f;
    [SerializeField] private Vector3 offset = new Vector3(-2, 2, 0);
    [SerializeField] private KeyCode toggleKeyChar;
    [SerializeField] private float floatFrequency = 1;

    public bool canCall;
    public bool companionIsFollowing = false;
    public bool isActive = false;
    private Vector3 leftPos;
    private int mod;

    void Start()
    {
        player = FindObjectOfType<Player>();
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null) {
            Debug.Log("Missing SpriteRenderer for companion");
        }
        leftPos = transform.position;
        canCall = false;
        am = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        toggleCompanion();
        if (companionIsFollowing) {
            float xoffset = transform.position.x - player.transform.position.x;
            renderer.flipX = xoffset < 0;
            mod = xoffset > 0 ? -1 : 1;
            companionFollow(player.transform.position);
        } 
        else
        {
            companionFollow(leftPos);
        }
    }

    /*private void companionFloat() {
        float distance = 1f;
        float ySpeed = 0.5f;

        Vector3 companionPos = transform.position;

        //has companion float up and down
        companionPos.y = companionPos.y + Mathf.Sin(Time.time);
        transform.position = companionPos;
    }*/

    //private float sinTime;
    private void companionFollow(Vector3 target) {

        Vector3 targetPos = new Vector3(target.x + offset.x * mod,
            target.y + offset.y + Mathf.Sin(Time.time * floatFrequency),
            target.z + offset.z);

        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 l = Vector3.Lerp(transform.position, targetPos, movePercent * distance * Time.deltaTime);
        transform.position = l;
    }

    private void toggleCompanion() {
        if (Input.GetKeyDown(toggleKeyChar) && canCall)
        {
            companionIsFollowing = !companionIsFollowing;
            leftPos = transform.position;
            if (companionIsFollowing)
                am.Play("leavePet");
            else
                am.Play("callPet");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canCall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            canCall = false;
        }
    }
}
