using System;
using UnityEngine;

public class SemiAuto : Gun
{
    private bool fired = false;

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    protected override void Shoot()
    {
        if(!fired)
        {
            heat = Mathf.Min(recoilPattern.Length - 1, heat + 1);
            heatCooldownTimer = heatCooldown;
            roundsRemaining--;

            SimulateBullet();

            fired = true;
        }
    }

    public override void Released()
    {
        fired = false;
    }

    public override void update(float dt)
    {
        base.update(dt);
    }

    public override Type GetLowerType()
    {
        return typeof(SemiAuto);
    }
}
