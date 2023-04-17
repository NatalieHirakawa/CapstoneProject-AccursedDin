using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Parallax : MonoBehaviour
{
    private Vector3 startPos;
    private Vector2 bounds;
    [SerializeField] private GameObject cam;
    public Vector2 parallaxEffect;
    void Awake()
    {
        startPos = transform.position;
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        if(r == null)
        {
            Tilemap t = GetComponent<Tilemap>();
            bounds = t.localBounds.center;
        }
        else
        {
            bounds = r.bounds.size;
        }
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        //SceneManager.sceneLoaded += this.OnLoadCallback;
    }

	/*void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
    }*/

    // Update is called once per frame
    void Update()
    {
        Vector2 dist = new Vector2(cam.transform.position.x * parallaxEffect.x,
            cam.transform.position.y * parallaxEffect.y);

        transform.position = new Vector3(startPos.x + dist.x - bounds.x/2, startPos.y + dist.y - bounds.y/2, transform.position.z);
    }
}
