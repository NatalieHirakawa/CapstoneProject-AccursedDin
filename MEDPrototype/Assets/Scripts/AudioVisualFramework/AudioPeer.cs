using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

/**
 * Author: Chris Wormald
 * This component should be applied to a single audio source emitter for retriving
 * The spectrum data without location interfering
 * If you want more than one emitter, you'll need more mixer channels
 * for the locational element, cuz otherwise location screws with the spectrum data
 */

public class AudioPeer : MonoBehaviour {

    public float spectrumValue { get; private set; }
    [HideInInspector] public int audioFidelity = 2048;
    [HideInInspector] public float freqperband = 22050f / 2048; //Hz
    public float[] m_audioSpectrum { get; private set; }
    public float[] m_audioWave { get; private set; }
    public AudioSource source;
    private GameObject primaryListener;
    public static GameObject peerObject { get; private set; }
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private new Light2D light;
    private string MIXER_CHANNEL;
    public float minListenDistance;
    public float maxListenDistance;
    [SerializeField] private bool isSpatial;
    public float multiplier = 100f;
    [SerializeField] private float step = 0.1f;
    [SerializeField] private uint maxAvergingBand = 50;
    [SerializeField] private float intensityMod = 20;
    [SerializeField] private float lightSpeed = 0.5f;

    [HideInInspector]
    public float listenRange { get; private set; }

    void Start() {
        light = this.GetComponent<Light2D>();
        listenRange = this.GetComponent<CircleCollider2D>().radius;
        m_audioSpectrum = new float[audioFidelity];
        peerObject = this.gameObject; //This is very crappy approach
        MIXER_CHANNEL = GetComponent<AudioSource>().outputAudioMixerGroup.name;
        if (isSpatial)
        {
            var listener = GameObject.FindObjectOfType<AudioListener>();
            primaryListener = listener.gameObject;
        }
    }

    void Update()
    {
        source.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        if (isSpatial)
        {
            if (primaryListener == null)
            {
                primaryListener = GameObject.FindObjectOfType<AudioListener>().gameObject;
            }
            else
            {
                Vector3 lisPos = primaryListener.transform.position;
                mixer.SetFloat(MIXER_CHANNEL, getVolStrength(lisPos));
            }
        }
        if(light != null)
            updateLight();
  
    }

    void updateLight()
    {
        float spectrumAvg = 0;
        for(int i = 0; i < maxAvergingBand; i++)
        {
            spectrumAvg += m_audioSpectrum[i];
        }
        spectrumAvg = spectrumAvg * intensityMod / maxAvergingBand;
        float intensity = Mathf.Lerp(light.intensity, spectrumAvg, lightSpeed * Time.deltaTime);
        light.intensity = intensity;
    }

    void generateSineWave()
    {
        for (int i = 0; i < audioFidelity/2; i++) { 
            float sinVal = 0;
            float sinX = i * step;
            for (uint j = 0; j < audioFidelity; j++)
            {
                sinVal += m_audioSpectrum[j] * Mathf.Sin(j * freqperband * sinX);
            }
            m_audioWave[i] = sinVal;
        }
    }

    float getVolStrength(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, this.gameObject.transform.position);
        float value = Mathf.Lerp(1, 0.0001f, (distance - minListenDistance) / maxListenDistance);
        return Mathf.Log10(value) * 20;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        VirtualListener lis;
        //Debug.Log(collision.gameObject);
        if ((lis = collision.GetComponent<VirtualListener>()) != null)
        {
            if (lis.source == this || lis.source == null)
                lis.sources.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        VirtualListener lis;
        if ((lis = collision.GetComponent<VirtualListener>()) != null)
        {
            if (lis.source == this || lis.source == null)
                lis.sources.Remove(this);
        }
    }
}
