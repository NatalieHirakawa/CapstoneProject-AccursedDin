using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W
//This is meant to be the main parent of all audio-response objects
//the reason for some discrepencies is that this was originally abstract

public class VirtualListener : MonoBehaviour
{

    public static float dampPercent = 0.5f;
    public static float dampTimeFactor = 1.1f;

    private SpriteRenderer sRen;
    private MeshRenderer mRen;
    private Color normalColor;
    private Color dampendColor;


    [HideInInspector] public List<AudioPeer> sources;
    [SerializeField] public AudioPeer source;
    [SerializeField] protected uint frequencyBandA;
    [SerializeField] protected uint frequencyBandB;
    //bool inRange = false;
    private float _audioVal;

    protected int freqToSpectrum(float freq)
    {
        int arrlen = source.audioFidelity;
        float index = freq * arrlen / 22050f;
        return (int)index - 1;
    }

    /*public void setInRange(bool b) // this is dumb and what 3am's will do to you
    {
        inRange = b;
    }*/

    public float getAudioVal()
    {
        return _audioVal;
    }

    protected virtual void OnUpdate(float spectrumValue) { return;  }

    void Update()
    {
        if (sources.Count > 0)
        {
            updateColor(normalColor);
            uint low = System.Math.Min(frequencyBandA, frequencyBandB);
            uint up = System.Math.Max(frequencyBandA, frequencyBandB);
            float sum = 0;
            foreach (AudioPeer s in sources) {
                for (uint i = low; i <= up; i++)
                    sum += s.m_audioSpectrum[i] * s.multiplier;
            }
            float audioVal = (sum / (up - low + 1));
            _audioVal = audioVal; //seem stupid? thats cuz it is.
            OnUpdate(audioVal); //seperate to allow overriding
        } else
        {
            _audioVal = 0;
            updateColor(dampendColor);
        }
    }

    private void updateColor(Color c)
    {
        if(sRen != null)
        {
            sRen.color = Color.Lerp(sRen.color, c, dampTimeFactor*Time.deltaTime);
        } else if (mRen != null)
        {
            mRen.material.color = Color.Lerp(mRen.material.color, c, dampTimeFactor * Time.deltaTime);
        }
    }

    private void Start()
    {
        
        sRen = GetComponent<SpriteRenderer>();
        if (sRen == null)
        {
            mRen = GetComponent<MeshRenderer>();
            if (mRen != null)
            {
                normalColor = mRen.material.color;
            }
            else
                return;
        }
        else
            normalColor = sRen.color;
        dampendColor = new Color(normalColor.r*dampPercent, normalColor.g* dampPercent, normalColor.b* dampPercent, normalColor.a);
    }
}
