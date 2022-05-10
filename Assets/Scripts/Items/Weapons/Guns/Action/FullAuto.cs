using System;
using UnityEngine;

public class FullAuto : Gun
{
    protected override void Shoot()
    {
        heat = Mathf.Min(recoilPattern.Length - 1, heat + 1);
        heatCooldownTimer = heatCooldown;
        roundsRemaining--;

        SimulateBullet();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    public override void Released() {}


    public override void update(float dt)
    {
        base.update(dt);
    }

    public override Type GetLowerType()
    {
        return typeof(FullAuto);
    }
}
