using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFind : MonoBehaviour
{
    private bool isFollowing;
    public float followSpeed;
    public GameObject key;
    public Transform followTarget;

    // Start is called before the first frame update
    void Start()
    {
        isFollowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            key.transform.position = Vector3.Lerp(transform.position, followTarget.position, followSpeed * Time.deltaTime);
        }
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Key>())
        {
            if (!isFollowing)
            {
                // GameObject follow = GameObject.FindWithTag("KeySpace");

                // followTarget = follow.transform;

                isFollowing = true;
            }
        }
    }
}
