using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W.

public class LevelFade : MonoBehaviour {
    [SerializeField] public Animator anim;

    private System.Action functionToCall;

    public void forceFadeIn()
    {
        anim.Play("Fade_in");
    }

    public void FadeToBlack(System.Action method)
    {
        anim.SetTrigger("Fade_out");
        functionToCall = method;
    }

    public void OnFadeComplete()
    {
        functionToCall();
    }
}
