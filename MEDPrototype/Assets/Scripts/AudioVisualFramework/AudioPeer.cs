using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/**
 * Author: Chris Wormald
 * This component should be applied to a single audio source emitter for retriving
 * The spectrum data without location interfering
 * If you want more than one emitter, you'll need more mixer channels
 * for the locational element, cuz otherwise location screws with the spectrum data
 */

public class AudioPeer : MonoBehaviour {

    public static float spectrumValue { get; private set; }
    public static int audioFidelity = 2048;
    public static float[] m_audioSpectrum { get; private set; }
    public AudioSource source;
    private GameObject primaryListener;
    public static GameObject peerObject { get; private set; }
    [SerializeField] private AudioMixer mixer;
    const string MIXER_CHANNEL = "MusicEmitter";
    public float minListenDistance;
    public float maxListenDistance;
    [SerializeField] private bool isSpatial;
    public static float multiplier = 100f;


    void Start() {
        m_audioSpectrum = new float[audioFidelity];
        peerObject = this.gameObject; //This is very crappy approach
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
            lis.setInRange(true);
        }
        if (collision.gameObject.name == "Player")
        {
            //this.GetComponent<Companion>().canCall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        VirtualListener lis;
        if ((lis = collision.GetComponent<VirtualListener>()) != null)
        {
            lis.setInRange(false);
        }
        if (collision.gameObject.name == "Player")
        {
            //this.GetComponent<Companion>().canCall = false;
        }
    }
}
