using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;
    //Collider2D coll;

    private float[] audioSpectrum;
    private bool spectrumValid;
    private Vector3[] vertices;
    private int[] triangles;

    //[SerializeField] private AudioPeer peer;
    private uint vertCount; // number of vertices (2x)
    [SerializeField] private float vertUnits; // units to increase along the x axis
    [SerializeField] private float vertHeight; // base y axis height
    [SerializeField] [Range(0, 512)] private int bandVis;
    private uint triangleCount;
    [SerializeField] private bool debug;
    public float valueAtDebug;

    private void OnDrawGizmos()
    {
        // Bounds
        if(!Application.isPlaying) { return; }

        Gizmos.color = Color.green;
        Vector3 org = new Vector3(bandVis * vertUnits, vertHeight + audioSpectrum[bandVis] * AudioPeer.multiplier, 0);
        Gizmos.DrawSphere(transform.position + org, 0.3f);
        valueAtDebug = audioSpectrum[bandVis] * AudioPeer.multiplier;
        if (debug)
            Debug.Log(valueAtDebug);
    }

    void Start() 
    {
        mesh = new Mesh();
        //coll = new Collider2D();
        GetComponent<MeshFilter>().mesh = mesh;
        vertCount = (uint) AudioPeer.audioFidelity;
        triangleCount = (vertCount - 1) * 2;
        audioSpectrum = new float[AudioPeer.audioFidelity];
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
    }

    void CreateShape()
    {
        for (int i = 0; i < vertices.Length/2; i++)
        {
            vertices[2*i] = new Vector3(i*vertUnits, 0, 0);
            vertices[2*i+1] = new Vector3(i*vertUnits, vertHeight + audioSpectrum[i] * AudioPeer.multiplier, 0);
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
