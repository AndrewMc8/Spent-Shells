using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossKilled : TriggerEvent
{
    [SerializeField] protected AudioSource VictorySound;

    public override void Trigger()
    {
        AudioSource.PlayClipAtPoint(VictorySound.clip, transform.position);
    }
}
