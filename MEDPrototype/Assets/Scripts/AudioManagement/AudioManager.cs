using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] clips; //this is bad and sad but unity is also bad and sad atm
    public string[] names;

    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }    
        DontDestroyOnLoad(gameObject);
        generateFromClips();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void generateFromClips()
    {
        if(names.Length != clips.Length)
        {
            Debug.LogError("Clips length does not match names length");
            return;
        }
        sounds = new Sound[names.Length];
        for(int i = 0; i < names.Length; i++)
        {
            sounds[i] = gameObject.AddComponent<Sound>();
            sounds[i].name = names[i];
            sounds[i].clip = clips[i];
            sounds[i].volume = 1;
            sounds[i].pitch = 1;
            sounds[i].speed = 1;
            sounds[i].loop = false;
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError(name + " audio sound was not found in this audio manager.");
            return;
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.Play();
    }
}
