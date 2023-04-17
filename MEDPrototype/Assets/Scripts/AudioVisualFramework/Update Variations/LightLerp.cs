using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

//Chris W

[RequireComponent(typeof(Light2D))]
public class LightLerp : VirtualListener
{
    [SerializeField] private float lightSpeed;
    [SerializeField] private float intensityMod;
    private new Light2D light;
    private Animator a;

    protected override void OnUpdate(float spectrum)
    {
        float intensity = Mathf.Lerp(light.intensity, spectrum*intensityMod, lightSpeed * Time.deltaTime);
        light.intensity = intensity;
        a.SetFloat("speed", intensity / intensityMod);
    }

    private void Awake()
    {
        light = GetComponent<Light2D>();
        a = GetComponent<Animator>();
    }
}
