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

    [Header("Base")]
    [SerializeField] protected float fireInterval = 0.2f;
    [SerializeField] protected float maxRange = 100;
    [SerializeField] protected bool chamberable = true;
    [SerializeField] protected int magCapacity = 12;
    [SerializeField] protected int roundsRemaining = 11;
    [SerializeField] protected float reloadTime = 2.0f;
    [SerializeField] protected HeatMethod heatMethod = HeatMethod.EXACT;
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
    [SerializeField] protected Transform gunPort;
    [SerializeField] protected GameObject gunBase;

    public void Validate()
    {
        OnValidate();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (fireInterval < 0) fireInterval = 0;

        if (magCapacity < 1) magCapacity = 1;
        if (roundsRemaining < 0) roundsRemaining = 0;

        if (roundsRemaining > magCapacity + ((chamberable) ? 1 : 0)) roundsRemaining = magCapacity + ((chamberable) ? 1 : 0);
        chambered = (roundsRemaining > magCapacity);

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

            print("Count: " + bulletLogics.Count);

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
                if (missFiredSound) AudioSource.PlayClipAtPoint(missFiredSound.clip, transform.position);
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
                if(reloadedSound) AudioSource.PlayClipAtPoint(reloadedSound.clip, transform.position);
                roundsRemaining = magCapacity + ((chamberable && roundsRemaining > 0) ? 1 : 0);
            }
        }
    }

    protected abstract void Shoot();

    protected void SimulateBullet()
    {
        Vector3 dir = gunPort.forward;

        if (heatMethod == HeatMethod.DEVIATION)
        {
            dir.x += Random.Range(-recoilPattern[(int)heat].x, recoilPattern[(int)heat].x);
            dir.y += Random.Range(-recoilPattern[(int)heat].y, recoilPattern[(int)heat].y);
        }
        else
        {
            dir.x += recoilPattern[(int)heat].x;
            dir.y += recoilPattern[(int)heat].y;
        }

        List<GameObject> hitsObjects = bulletLogic.GenerateHits(gunPort.position, dir, maxRange);
        foreach(var go in hitsObjects)
        {
            Damage(go);
        }
    }
}
