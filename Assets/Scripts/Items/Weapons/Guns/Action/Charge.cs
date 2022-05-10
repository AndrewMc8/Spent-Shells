using System;
using UnityEngine;

public class Charge : Gun
{
    [Header("Charge Specific")]
    [SerializeField] private float maxCharge;
    [SerializeField] private float minCharge;
    [SerializeField] private float chargeRate;
    [SerializeField] private float decayRate;
    [SerializeField] private float minShoot;
    [SerializeField] private float chargeLoss;

    private float currentCharge;
    private bool charge = false;

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    public override void update(float dt)
    {
        base.update(dt);

        if (charge)
            currentCharge = Mathf.Min(currentCharge + chargeRate * dt, maxCharge);
        else if (currentCharge > minCharge) 
            currentCharge = Mathf.Max(Mathf.Min(currentCharge - decayRate * dt, currentCharge), minCharge);
        
        charge = false;
    }

    public override void Pressed()
    {
        if(currentCharge >= minShoot) Shoot();
    }

    public override void Reload()
    {
        charge = true;
    }

    protected override void Shoot()
    {
        currentCharge = (chargeLoss == 0) ? 0 : Mathf.Max(currentCharge - chargeLoss, 0);
        SimulateBullet();
    }

    public override void Released() {}

    public override Type GetLowerType()
    {
        return typeof(Charge);
    }
}
