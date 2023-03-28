using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SineGen : MonoBehaviour {

    Mesh mesh;
    //Collider2D coll;

    private float[] audioSpectrum;
    private bool spectrumValid;
    private Vector3[] vertices;
    private int[] triangles;

    //[SerializeField] private AudioPeer peer;
    private uint fidelity;
    private float freqperband;
    [SerializeField] private uint vertCount = 100; // number of vertices (2x)
    [SerializeField] private float vertUnits; // units to increase along the x axis
    [SerializeField] private float vertHeight; // base y axis height
    [SerializeField] private uint sampleEvery = 2;
    [SerializeField] private uint maxBand = 200;
    [SerializeField] private uint myMult = 10;
    private uint triangleCount;

    void Start()
    {
        mesh = new Mesh();
        //coll = new Collider2D();
        GetComponent<MeshFilter>().mesh = mesh;
        fidelity = (uint)AudioPeer.audioFidelity;
        triangleCount = (vertCount - 1) * 2;
        audioSpectrum = new float[AudioPeer.audioFidelity];
        vertices = new Vector3[vertCount * 2];
        triangles = new int[triangleCount * 3];
        freqperband = 22050f / fidelity; //Hz
        CreateTriangles();
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
        for (int i = 0; i < vertices.Length / 2; i++)
        {
            float sinVal = 1;
            float x = i * vertUnits;
            for (uint j = 0; j < maxBand; j += sampleEvery)
            {
                sinVal *= Mathf.Sin(x + j * freqperband) * audioSpectrum[i] * myMult;
            }
            sinVal *= myMult;
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
        audioSpectrum = AudioPeer.m_audioSpectrum;
        spectrumValid = audioSpectrum != null && audioSpectrum.Length > 0;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

}
