using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W

public class Bullet : MonoBehaviour
{

    private GameObject player;
    private Rigidbody2D rb;
    private CircleCollider2D cc2d; 
    public float force;
    private float timer;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float lifetime = 10;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > lifetime)
        {
            Destroy(gameObject);
        }
        Collider2D overlap = Physics2D.OverlapCircle(transform.position, cc2d.radius, mask); //this is dumb but works at least
        if(overlap != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") //idk why terrain just wouldnt work here
        {
            Destroy(gameObject);
        }
    }
}
