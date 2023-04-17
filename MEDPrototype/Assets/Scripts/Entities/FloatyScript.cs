using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyScript : MonoBehaviour
{
    private Vector3 origin;
    [SerializeField] private float offset = 1;
    [SerializeField] private float freq = 1;
    [SerializeField] private bool randomize = true;

    private void Start()
    {
        origin = transform.position;
        if (!randomize) return;
        offset = Random.value * offset;
        freq = Random.value * freq + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(origin.x, origin.y + 0.5f * Mathf.Sin(freq * Time.time + offset), origin.z);
    }
}
