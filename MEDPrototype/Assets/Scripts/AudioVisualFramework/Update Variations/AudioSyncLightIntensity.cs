using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

//Chris W

[RequireComponent(typeof(Light2D))]
public class AudioSyncLightIntensity : VLThreshold
{
    [SerializeField] private float beatIntensity;
    private float baseintensity;
    private new Light2D light;

    private IEnumerator MoveToScale(float _target)
    {
        float _curr = light.intensity;
        float _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Mathf.Lerp(_initial, _target, _timer / timeToWait);
            _timer += Time.deltaTime;
            light.intensity = _curr;

            yield return null;
        }

        PassedThreshold = false;
    }

    protected override void OnUpdate(float audioVal)
    {
        base.OnUpdate(audioVal);

        if (PassedThreshold) return;
        light.intensity = Mathf.Lerp(light.intensity, baseintensity, restSmoothTime * Time.deltaTime);
    }

    public override void OnThreshold()
    {
        //Debug.Log("Did something");
        base.OnThreshold();
        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatIntensity);
    }

    private void Awake()
    {
        light = GetComponent<Light2D>();
        baseintensity = light.intensity;
    }
}
