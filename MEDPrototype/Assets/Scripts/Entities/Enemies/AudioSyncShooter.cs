using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Chris W

public class AudioSyncShooter : VLThreshold
{

    public GameObject bullet;
    public Transform bulletPos;

    private GameObject player;
    public static float GhostShootRange = 20f;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public override void OnThreshold()
    {
        base.OnThreshold();
        if(Vector3.Distance(player.transform.position, this.transform.position) < GhostShootRange)
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    protected override void OnUpdate(float audioVal)
    {
        base.OnUpdate(audioVal);

        if (audioVal > threshold)
        {
            float scalex = Mathf.Abs(transform.localScale.x) * Mathf.Sign(player.transform.position.x - transform.localPosition.x);
            transform.localScale = new Vector3(scalex, transform.localScale.y, transform.localScale.z);
        }
    }

}
