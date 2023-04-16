using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SineGen : VirtualListener {

    Mesh mesh;
    //Collider2D coll;

    //private float[] audioSpectrum;
    private bool spectrumValid;
    private Vector3[] vertices;
    private int[] triangles;

    //[SerializeField] private AudioPeer peer;
    private uint fidelity;
    private float freqperband;
    [Tooltip("How many x-axis vertices. Keep in mind this number is doubled for the bottom verts")]
    [SerializeField] private uint vertCount = 100; // number of vertices (2x)
    [Tooltip("The distance between each vertex pair")]
    [SerializeField] private float vertUnits; // units to increase along the x axis
    [Tooltip("The base height of the block.")]
    [SerializeField] private float vertHeight; // base y axis height
    [Tooltip("Resolution of spectrum analized, keep low")]
    [SerializeField] private uint sampleEvery = 2;
    [Tooltip("The furthest frequency to check. Performance optimization since higher frequencies generally have lower effect")]
    [SerializeField] private uint maxBand = 200;
    [Tooltip("How much to multiply the sine wave by vertically.")]
    [SerializeField] private uint heightMultiplier = 2;
    private uint triangleCount;
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .5f);
        Vector3 center = new Vector3(transform.position.x + vertUnits * vertCount / 4, transform.position.y + vertHeight / 2, transform.position.z);
        Vector3 size = new Vector3(vertUnits * vertCount / 2, vertHeight, 0);
        Gizmos.DrawCube(center, size);
    }
    void Start()
    {
        mesh = new Mesh();
        //coll = new Collider2D();
        GetComponent<MeshFilter>().mesh = mesh;
        fidelity = (uint)FindObjectOfType<AudioPeer>().audioFidelity;
        triangleCount = (vertCount - 1) * 2;
        //audioSpectrum = new float[source.audioFidelity];
        vertices = new Vector3[vertCount * 2];
        triangles = new int[triangleCount * 3];
        freqperband = 22050f / fidelity; //Hz
        CreateTriangles();
        CreateShape();
        UpdateMesh();
        //Debug.Log(freqperband);
    }

    void Update()
    {
        //GetSpectrum();
        if (sources.Count > 0)
        {
            CreateShape();
            UpdateMesh();
       
              //  GetComponent<CustomCollider2D>(). = null;
              //  GetComponent<MeshCollider>().sharedMesh = mesh;
             
        }
    }

    [Tooltip("The step resolution of the sine wave on the x axis. Lower is higher resolution. Keep in mind that a wave repeats, so too high will make a very repetative noisey surface. Too low will capture less of the wave.")]
    [SerializeField] private float step;

    void CreateShape()
    {
        for (int i = 0; i < vertices.Length / 2; i++)
        {
            float sinVal = 0;
            float x = i * vertUnits;
            foreach (AudioPeer s in sources){
                float sinX = i * step;
                for (uint j = 0; j < maxBand; j += sampleEvery)
                {
                    sinVal += s.m_audioSpectrum[j] * Mathf.Sin(j * freqperband * sinX);
                }
                sinVal *= heightMultiplier;
            }

            vertices[2 * i] = new Vector3(x, 0, 0);
            vertices[2 * i + 1] = new Vector3(x, vertHeight + sinVal, 0);
        }

    }

    void CreateTriangles()
    {
        for (int i = 0; i < vertices.Length / 2; i += 2)
        {
            int v = i * 3;
            //triangle bottom left
            triangles[v] = i;
            triangles[v + 1] = i + 1;
            triangles[v + 2] = i + 2;

            //triangle top right
            triangles[v + 3] = i + 1;
            triangles[v + 4] = i + 3;
            triangles[v + 5] = i + 2;
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
        //audioSpectrum = source.m_audioSpectrum;
        //spectrumValid = audioSpectrum != null && audioSpectrum.Length > 0;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

}
