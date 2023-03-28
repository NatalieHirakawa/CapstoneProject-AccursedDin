using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float FollowSpeed;
    [SerializeField] private float yOffset;
    [SerializeField] private Transform target;

    // Update is called once per frame
    void Update() {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
