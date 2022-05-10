using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckShot : BulletLogic
{
    protected void OnValidate()
    {
        if(gameObject.transform.TryGetComponent<Gun>(out Gun gun))
        {
            gun.Validate();
        }
    }

    public override List<GameObject> GenerateHits()
    {
        throw new System.NotImplementedException();
    }
}
