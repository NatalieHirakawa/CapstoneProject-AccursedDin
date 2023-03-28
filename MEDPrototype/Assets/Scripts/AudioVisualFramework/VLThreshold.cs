using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auth: Chris W.
//

public class VLThreshold : VirtualListener {
    [SerializeField] protected float threshold;
    [SerializeField] protected float timeStep;
    [SerializeField] protected float timeToWait;
    [SerializeField] protected float restSmoothTime; //not always used

    private float m_prevAudioVal;
    private float m_timer;

    [HideInInspector]
    public bool PassedThreshold; //we also have this for noninherited approaches. its dumb but allows for "multiple inheritance"
    //I need to actually learn CSharp heheh cuz theres definitely better ways for all this crap
    
    public virtual void OnThreshold()
    {
        m_timer = 0;
        PassedThreshold = true;
    }

    protected override void OnUpdate(float audioVal)
    {
        //previous code but kept for reference as of now
        //m_prevAudioVal = audioVal;
        /*if (m_prevAudioVal > threshold && audioVal <= threshold)
        {
            if (m_timer > timeStep)
                OnThreshold();
        }

        if (m_prevAudioVal <= threshold && audioVal > threshold)
        {
            if (m_timer > timeStep)
                OnThreshold();
        }*/

        if (m_prevAudioVal <= threshold && audioVal > threshold)
        {
            if (m_timer > timeStep)
                OnThreshold();
        }
        m_prevAudioVal = audioVal;


        m_timer += Time.deltaTime;
    }
}
