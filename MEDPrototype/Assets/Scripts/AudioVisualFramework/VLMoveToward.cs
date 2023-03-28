using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Auth: Chris W.
//

public class VLMoveToward : VirtualListener {
    [SerializeField] protected float threshold;
    private GameObject player;
    [SerializeField] float moveSpeed;

    protected bool m_isBeat;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    public virtual void OnThreshold()
    {
        m_isBeat = true;
    }

    protected override void OnUpdate(float audioVal)
    {

        if (audioVal > threshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.localPosition, moveSpeed * Time.deltaTime);
            float scalex = Mathf.Abs(transform.localScale.x) * Mathf.Sign(player.transform.position.x - transform.localPosition.x);
            transform.localScale = new Vector3(scalex, transform.localScale.y, transform.localScale.z);
        }
    }
}
