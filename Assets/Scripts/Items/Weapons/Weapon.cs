using Assets.Scripts.Items.Weapons;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [Header("Weapon Base")]
    [Tooltip("Damage is for Each Projectile")]
    [SerializeField] protected List<Damage> damages = new List<Damage>();

    public abstract void Pressed();
    public abstract void Released();
    public abstract void update(float dt);

    protected override void OnValidate()
    {
        base.OnValidate();
    }

    public override System.Type GetBaseType()
    {
        return typeof(Weapon);
    }

    public void Damage(Collider collider)
    {
        if(LayerMask.LayerToName(collider.gameObject.layer).Equals("WeakPoint"))
        {
            if (collider.gameObject.transform.root.TryGetComponent<Character>(out Character character))
                character.WeakPointHit(collider);
            else
                collider.gameObject.SetActive(false);
        }

        if (collider.gameObject.transform.root.TryGetComponent<Health>(out Health health))
        {
            damages.ForEach(x => health.Damage(new Damage(x), collider.tag.Contains("colliderMulti: ") ? float.Parse(collider.tag.Substring("colliderMulti: ".Length - 1)) : 1));
        }
    }

    public void Damage(GameObject hitObject)
    {
        if (hitObject.transform.root.TryGetComponent<Health>(out Health health))
        {
            damages.ForEach(x => health.Damage(new Damage(x), hitObject.tag.Contains("colliderMulti: ") ? float.Parse(hitObject.tag.Substring("colliderMulti: ".Length - 1)) : 1));
        }
    }

    public abstract System.Type GetLowerType();

    public override string GetDesc()
    {
        float dmg = 0;
        foreach(var d in damages)
        { 
            dmg += d.damage / ((d.duration > 0) ? d.duration : 1);
        }

        return GetLowerType().ToString() + " DMG: " + dmg;
    }
}
