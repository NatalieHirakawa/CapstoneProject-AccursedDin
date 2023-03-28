using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W
//Ai Le

public class Companion : MonoBehaviour
{
    [SerializeField] private Player player;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private SpriteRenderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    //renderer stuff referenced from:
    //https://levelup.gitconnected.com/sprite-flipping-in-unity-for-2d-animations-f5c33d3e8f71
    //https://answers.unity.com/questions/952558/how-to-flip-sprite-horizontally-in-unity-2d.html
    [SerializeField] private float movePercent = 0.75f;
    [SerializeField] private Vector3 offset = new Vector3(-2, 2, 0);

    public bool canCall;
    private bool companionIsFollowing = false;
    public bool isActive = false;
    private Vector3 leftPos;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null) {
            Debug.Log("Missing SpriteRenderer for companion");
        }
        leftPos = transform.position;
        canCall = false;
        companionIsFollowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        toggleCompanion();
        if (companionIsFollowing) {
            companionFollow(player.transform.position);
            leftPos = transform.position;
        } else
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
        bool playerFacing = player.GetComponent<SpriteRenderer>().flipX; // true = facing negative x
        renderer.flipX = playerFacing;
        int mod = playerFacing ? -1 : 1;
        Vector3 targetPos = new Vector3(target.x + offset.x * mod,
            target.y + offset.y + Mathf.Sin(Time.time),
            target.z + offset.z);

        float distance = Vector3.Distance(targetPos, transform.position);
        Vector3 l = Vector3.Lerp(transform.position, targetPos, movePercent * distance * Time.deltaTime);
        transform.position = l;
    }

    private void toggleCompanion() {
        //toggle companion on/off with Q
        /**if (Input.GetKeyDown(KeyCode.Q)) {//I will refrain for now
            switch(canCall) {//idk why this switch statement exists
                case true:
                    //canCall = false;
                    isActive = false;
                    break;
                case false:
                    //canCall = true;
                    isActive = true;
                    break;
            }
        }*/

        //player tells companion to stay put with E
        if (Input.GetKeyDown(KeyCode.E)){//&& canCall) {
            companionIsFollowing = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))// && canCall)
        {
            companionIsFollowing = false;
        }
        //Debug.Log(canCall);
    }
}
