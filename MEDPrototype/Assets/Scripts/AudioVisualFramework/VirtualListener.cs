using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W
//This is meant to be the main parent of all audio-response objects
//the reason for some discrepencies is that this was originally abstract

public class VirtualListener : MonoBehaviour
{
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
        }
    }

    /*private void Start()
    {
        inRange = false;
    }*/
}
