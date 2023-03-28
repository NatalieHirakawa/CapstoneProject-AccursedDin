using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorUnlockTrack : MonoBehaviour
{
    public static DoorUnlockTrack instance;
    
    public bool Level1Complete = false;
    public bool Level2Complete = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


}
