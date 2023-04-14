using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SpectrumGen : MonoBehaviour
{

    Mesh mesh;
    //Collider2D coll;

    private float[] audioSpectrum;
    private bool spectrumValid;
    private Vector3[] vertices;
    private int[] triangles;

    //[SerializeField] private AudioPeer peer;
    private uint vertCount; // number of vertices (2x)
    [SerializeField] private AudioPeer source;
    [SerializeField] private float vertUnits; // units to increase along the x axis
    [SerializeField] private float vertHeight; // base y axis height
    [SerializeField] [Range(2, 2048)] private uint maxBand = 2048; // base y axis height
    [SerializeField] [Range(0, 512)] private int bandVis;
    [SerializeField] private bool followCam;
    [SerializeField] private float mod;
    private Camera cam;
    private uint triangleCount;
    //[SerializeField] private bool debug;
    public float valueAtDebug;

    private void OnDrawGizmos()
    {
        float mult = Mathf.Min(source.audioFidelity, maxBand);
        Gizmos.color = new Color(1, 1, 1, .5f);
        Vector3 center = new Vector3(transform.position.x + vertUnits * mult / 2, transform.position.y + vertHeight / 2, transform.position.z);
        Vector3 size = new Vector3(vertUnits * mult, vertHeight, 0);
        Gizmos.DrawCube(center, size);

        // Bounds
        if (!Application.isPlaying) { return; }

        Gizmos.color = Color.green;
        Vector3 org = new Vector3(bandVis * vertUnits, vertHeight + audioSpectrum[bandVis] * source.multiplier, 0);
        Gizmos.DrawSphere(transform.position + org, 0.3f);
        valueAtDebug = audioSpectrum[bandVis] * source.multiplier;
       // if (debug)
        //    Debug.Log(valueAtDebug);
    }

    void Start() 
    {
        cam = FindObjectOfType<CameraFollow>().GetComponent<Camera>();
        mesh = new Mesh();
        //coll = new Collider2D();
        GetComponent<MeshFilter>().mesh = mesh;
        vertCount = (uint)source.audioFidelity;
        triangleCount = (vertCount - 1) * 2;
        audioSpectrum = new float[source.audioFidelity];
        vertices = new Vector3[vertCount*2];
        triangles = new int[triangleCount * 3];
        CreateTriangles();
    }

    void Update()
    {
        GetSpectrum(); 
        if(spectrumValid)
        {
            CreateShape();
            UpdateMesh();
        }
        if (followCam)
        {
            float xBound = -cam.aspect * cam.orthographicSize;
            float yBound = -cam.orthographicSize;
            transform.position = (Vector2) cam.transform.position + new Vector2(xBound, yBound);
        }
    }

    void CreateShape()
    {
        for (int i = 0; i < vertices.Length/2 && i < maxBand; i++)
        {
            vertices[2*i] = new Vector3(i*vertUnits, 0, 0);
            vertices[2*i+1] = new Vector3(i*vertUnits, vertHeight + audioSpectrum[i] * mod, 0);
        }
        
    }

    void CreateTriangles()
    {
        for (int i = 0; i < vertices.Length / 2; i += 2)
        {
            int v = i * 3;
            //triangle bottom left
            triangles[v] = i;
            triangles[v+1] = i+1;
            triangles[v+2] = i+2;

            //triangle top right
            triangles[v+3] = i+1;
            triangles[v+4] = i+3;
            triangles[v+5] = i+2;
        }
        /*string deb = "";
        for(int i = 0; i < triangles.Length; i++)
        {
            deb = deb + triangles[i] + ", ";
        }
        Debug.Log(deb);*/
    }

    void GetSpectrum()
    {
        audioSpectrum = source.m_audioSpectrum;
        spectrumValid = audioSpectrum != null && audioSpectrum.Length > 0;   
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

}
