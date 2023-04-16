using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Chris W
[System.Serializable]
public class Sound : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public string name;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public AudioSource source;

    public bool loop;

    public Sound(string name, AudioClip clip, float volume, float pitch, float speed, AudioSource source, bool loop)
    {
        this.name = name;
        this.clip = clip;
        this.volume = volume;
        this.pitch = pitch;
        this.speed = speed;
        this.source = source;
        this.loop = loop;
    }
}
