using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shotgun Specific")]
    [SerializeField] private int pelletCount = 5;

    private void OnValidate()
    {
        heatMethod = HeatMethod.DEVIATION;
    }

    protected override void Shoot()
    {
        roundsRemaining--;
        for(int i = 0; i < pelletCount; i ++)
        {
            heat = Mathf.Min(recoilPattern.Length - 1, heat + 1);
            heat = Mathf.Max(heat, 0);

//            SimulateBullet();
        }

        heat = 0;
    }

    public override void update(float dt)
    {
        base.update(dt);
    }

    public override void Released() {}

    public override Type GetLowerType()
    {
        return typeof(Shotgun);
    }
}
