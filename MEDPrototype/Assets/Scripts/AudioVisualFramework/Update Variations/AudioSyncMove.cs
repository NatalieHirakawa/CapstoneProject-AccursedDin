using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncMove : VLThreshold
{
    public Vector3 RelativePosition;
    private Vector3 RelativePos;
    private Vector3 RestPos;

    private IEnumerator MoveToPosition(Vector3 _target)
    {
        Vector3 _curr = transform.localPosition;
        Vector3 _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Vector3.Lerp(_initial, _target, _timer / timeToWait);
            _timer += Time.deltaTime;

            transform.localPosition = _curr;

            yield return null;
        }

        PassedThreshold = false;
    }

    protected override void OnUpdate(float audioVal)
    {
        base.OnUpdate(audioVal);

        if (PassedThreshold) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, RestPos, restSmoothTime * Time.deltaTime);
    }

    public override void OnThreshold()
    {
        base.OnThreshold();
        StopCoroutine("MoveToPosition");
        StartCoroutine("MoveToPosition", RelativePos);
    }

    public void Start()
    {
        RestPos = transform.localPosition;
        RelativePos = transform.localPosition + RelativePosition;
    }
}
