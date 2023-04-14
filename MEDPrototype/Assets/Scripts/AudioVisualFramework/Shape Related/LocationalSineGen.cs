using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LocationalSineGen : MonoBehaviour {

    Mesh mesh;
    //Collider2D coll;

    [SerializeField] private AudioPeer source;

    private float[] audioSpectrum;
    private bool spectrumValid;
    private Vector3[] vertices;
    private int[] triangles;

    [Tooltip("What to generate from.")]
    [SerializeField] private GameObject origin;
    [Tooltip("How wide the gap should be but halved")]
    [SerializeField] private float gapWidth;

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
    [Tooltip("The step resolution of the sine wave on the x axis. Lower is higher resolution. Keep in mind that a wave repeats, so too high will make a very repetative noisey surface. Too low will capture less of the wave.")]
    [SerializeField] private float step;

    private uint fidelity;
    private uint triangleCount;
    private uint startVertCount;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .5f);
        Vector3 center = new Vector3(transform.position.x + vertUnits * vertCount / 4, transform.position.y + vertHeight/2, transform.position.z);
        Vector3 size = new Vector3(vertUnits * vertCount / 2, vertHeight, 0);
        Gizmos.DrawCube(center, size);
    }

    void Start()
    {
        mesh = new Mesh();
        //coll = new Collider2D();
        GetComponent<MeshFilter>().mesh = mesh;
        fidelity = (uint)source.audioFidelity;
        triangleCount = (vertCount - 1) * 2;
        audioSpectrum = new float[source.audioFidelity];
        vertices = new Vector3[vertCount * 2];
        triangles = new int[triangleCount * 3];
        freqperband = 22050f / fidelity; //Hz
        CreateTriangles();
        startVertCount = vertCount;
    }

    void Update()
    {
        GetSpectrum();
        if (spectrumValid)
        {
            CreateShape();
            UpdateMesh();
        }
    }

    void CreateShape()
    {
        float srcX = origin.transform.position.x;
        for (int i = 0; i < vertices.Length / 2; i++)
        {
            float sinVal = 0;
            float x = i * vertUnits;
            float sinX;
            if (transform.position.x + x < srcX - gapWidth)
            {
                sinX = (srcX - gapWidth - (transform.position.x + x)) * step;
            } 
            else if(transform.position.x + x > srcX + gapWidth)
            {
                sinX = ((transform.position.x + x) - (srcX + gapWidth)) * step;
            } 
            else
            {
                vertices[2 * i] = new Vector3(x, 0, 0);
                vertices[2 * i + 1] = new Vector3(x, vertHeight, 0);
                continue;
            }
            for (uint j = 0; j < maxBand; j += sampleEvery)
            {
                sinVal += audioSpectrum[j] * Mathf.Sin(j * freqperband * sinX);
            }
            sinVal *= heightMultiplier;
            vertices[2 * i] = new Vector3(x, 0, 0);
            vertices[2 * i + 1] = new Vector3(x, vertHeight + sinVal, 0);
        }

    }

    Vector2 getBounds()
    {
        float x = transform.position.x;
        float y = transform.position.x + vertCount * vertUnits;
        return new Vector2(x, y);
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
