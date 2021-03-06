using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Gun : Weapon
{
    public enum HeatMethod
    {
        EXACT,
        DEVIATION
    }

    [SerializeField] protected int extraAmmo;

    [HideInInspector]
    public int AmmoCount { get { return extraAmmo; } }

    [Header("Base")]
    [SerializeField] protected float fireInterval = 0.2f;
    [SerializeField] protected float maxRange = 100;
    [SerializeField] protected bool chamberable = true;
    [SerializeField] protected int magCapacity = 12;
    [SerializeField] protected int roundsRemaining = 11;
    [SerializeField] protected float reloadTime = 2.0f;
    [SerializeField] protected HeatMethod heatMethod = HeatMethod.EXACT;
    [Tooltip("Recoil Pattern is After 10 units")]
    [SerializeField] protected Vector2[] recoilPattern;
    [SerializeField] protected float heatCooldown;
    [SerializeField] protected float heatCooldownRate;

    [SerializeField] protected BulletLogic bulletLogic = null;

    protected float heat = 0;
    protected float heatCooldownTimer = 0.0f;
    protected float fireTimer = 0.0f;
    protected float reloadTimer = 0.0f;
    protected bool chambered = false;

    [HideInInspector] public int MagCapacity { get { return magCapacity; } }
    [HideInInspector] public int RoundsRemaining { get { return roundsRemaining; } }

    [Header("Sound Effects")]
    [SerializeField] protected AudioSource firedSound = null;
    [SerializeField] protected AudioSource missFiredSound = null;
    [SerializeField] protected AudioSource reloadedSound = null;

    [Header("GunParts")]
    [SerializeField] protected Transform viewPort;
    [SerializeField] protected Transform gunPort;
    [SerializeField] protected GameObject gunBase;

    [SerializeField] protected GameObject muzzle_flash;

    public Transform ViewPort { get { return viewPort; } }
    public float Range { get { return maxRange; } }

    public void Validate()
    {
        OnValidate();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (fireInterval < 0) 
            fireInterval = 0;

        if (magCapacity < 1) 
            magCapacity = 1;
        
        if (roundsRemaining < 0) 
            roundsRemaining = 0;

        if (maxRange <= 0)
            maxRange = 1;

        if (reloadTime < 0)
            reloadTime = 0;

        if (roundsRemaining > magCapacity + ((chamberable) ? 1 : 0)) 
            roundsRemaining = magCapacity + ((chamberable) ? 1 : 0);

        if (heatCooldown < 0)
            heatCooldown = 0;

        if (heatCooldownRate < 0)
            heatCooldownRate = 0;

        if (extraAmmo < 0)
            extraAmmo = 0;

        if (recoilPattern == null || recoilPattern.Length < 1)
        {
            Array.Resize(ref recoilPattern, 1);
            recoilPattern[0] = Vector3.zero;
        }

        if(heatMethod == HeatMethod.DEVIATION)
        {
            for(int i = 0; i < recoilPattern.Length; i++)
            {
                recoilPattern[i].x = Mathf.Abs(recoilPattern[i].x);
                recoilPattern[i].y = Mathf.Abs(recoilPattern[i].y);
            }
        }

        if(bulletLogic == null)
        {
            if(gameObject.TryGetComponent<BulletLogic>(out BulletLogic foundBulletLogic))
            {
                bulletLogic = foundBulletLogic;
            }
            else
            {
                bulletLogic = gameObject.AddComponent<SingleShot>();
            }
        }
        else
        {
            List<BulletLogic> bulletLogics = gameObject.transform.GetComponents<BulletLogic>().ToList();

            switch (bulletLogics.Count)
            {
                case 0:
                    gameObject.AddComponent<SingleShot>();
                    break;
                case 1:
                    bulletLogic = bulletLogics[0];
                    break;
                default:
                    bulletLogic = bulletLogics[bulletLogics.Count - 1];
                    break;
            }
        }
    }

    public abstract override void Released();
    public override void Pressed() { Fire(); }

    public void Fire()
    {
        if(reloadTimer <= 0 && fireTimer <= 0)
        {
            if(roundsRemaining > 0)
            {
                fireTimer = fireInterval;

                Shoot();
            }
            else
            {
                Reload();
            }
        }
    }

    public virtual void Reload()
    {
        reloadTimer = reloadTime;
    }

    public override void update(float dt)
    {
        if (fireTimer > 0) fireTimer -= dt;
        if (heatCooldownTimer > 0) heatCooldownTimer -= dt;
        if (reloadTimer > 0)
        {
            reloadTimer -= dt;

            if(reloadTimer <= 0)
            {
                if(reloadedSound)
                    AudioSource.PlayClipAtPoint(reloadedSound.clip, transform.position);

                //  max request
                int requestedAmmo = magCapacity + ((chamberable && roundsRemaining > 0) ? 1 : 0) - roundsRemaining;
                requestedAmmo = Mathf.Min(requestedAmmo, extraAmmo);

                roundsRemaining += requestedAmmo;
                extraAmmo -= requestedAmmo;
            }
        }
    }

    protected abstract void Shoot();

    protected void SimulateBullet()
    {
        if(roundsRemaining > 0)
        {
            float xDev;
            float yDev;

            switch (heatMethod)
            {
                case HeatMethod.EXACT:
                    xDev = recoilPattern[(int)heat].x;
                    yDev = recoilPattern[(int)heat].y;
                    break;
                case HeatMethod.DEVIATION:
                    xDev = UnityEngine.Random.Range(-recoilPattern[(int)heat].x, recoilPattern[(int)heat].x);
                    yDev = UnityEngine.Random.Range(-recoilPattern[(int)heat].y, recoilPattern[(int)heat].y);
                    break;
                default:
                    throw new InvalidOperationException("How?");
            }

            Vector3 dir = (viewPort.forward * 10 + viewPort.right * -xDev + viewPort.up * yDev).normalized;

            if (firedSound)
                AudioSource.PlayClipAtPoint(firedSound.clip, transform.position);

            if (muzzle_flash)
            {
                muzzle_flash.transform.position = gunPort.transform.position;
                muzzle_flash.transform.forward = gunPort.transform.forward;
                muzzle_flash.GetComponent<ParticleSystem>().Play();
            }

            roundsRemaining--;
            List<GameObject> hitObjects = bulletLogic.GenerateHits(viewPort, dir, maxRange);

            foreach(var go in hitObjects)
            {
                Damage(go);
            }
        }
        else
        {
            if(missFiredSound)
                AudioSource.PlayClipAtPoint(missFiredSound.clip, transform.position);
        }

    }
}
