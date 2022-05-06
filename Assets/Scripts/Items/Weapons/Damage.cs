﻿using System;

namespace Assets.Scripts.Items.Weapons
{
    public enum DamageType
    {
        FIRE,
        COLD,
        ACID,
        FORCE,
        NECROTIC,
        PSYCHIC,
        POISON,
        RADIANT,
        PIERCING,
        ELECTRIC,
        BLUNT,
        SLASHING
    }

    [Serializable]
    public class Damage
    {
        public Damage() { }
        public Damage(Damage damage)
        {
            this.damageType = damage.damageType;
            this.damage = damage.damage;
            this.duration = damage.duration;
        }

        public DamageType damageType;
        public float damage;
        public float duration;
    }
}
