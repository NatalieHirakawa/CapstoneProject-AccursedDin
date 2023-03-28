using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move player on spawn of level

public class MovePlayer : MonoBehaviour
{

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject startDoor = GameObject.FindGameObjectWithTag("Start");

        player.transform.position = startDoor.transform.position;
        player.transform.position = new Vector3(startDoor.transform.position.x, startDoor.transform.position.y, 0);
    }
}
