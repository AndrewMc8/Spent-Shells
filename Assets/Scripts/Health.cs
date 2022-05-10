using Assets.Scripts.Items.Weapons;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 40;
    [SerializeField] protected float currentHealth = 40;

    [SerializeField] protected AudioSource hurtSound;
    [SerializeField] protected AudioSource deathSound;
    [SerializeField] protected AudioSource healSound;

    [HideInInspector] public float MaxHealth { get { return maxHealth; } }
    [HideInInspector] public float CurrentHealth { get { return currentHealth; } }

    private List<Damage> damages = new List<Damage>();

    public void update(float dt)
    {
        List<Damage> tempDamages = new List<Damage>();
        damages.ForEach(x => tempDamages.Add(x));

        tempDamages.ForEach(
            x =>
            {
                currentHealth = Mathf.Min(currentHealth - x.damage * dt, maxHealth);
                x.duration -= dt;

                if (x.duration <= 0) damages.Remove(x);
                if (currentHealth <= 0) Die();
            });
    }

    public void Damage(Damage damage, float dmgMulti = 1)
    {
        damage.damage *= dmgMulti;

        if (damage.duration == 0)
        {
            currentHealth = Mathf.Min(currentHealth - damage.damage, maxHealth);

            if (currentHealth > 0)
            {
                if (hurtSound) AudioSource.PlayClipAtPoint(hurtSound.clip, transform.position);
            }
            else Die();
        }
        else
            damages.Add(damage);
    }

    public void Heal(float hp)
    {
        currentHealth = Mathf.Min(currentHealth + hp, maxHealth);
        if (healSound) AudioSource.PlayClipAtPoint(healSound.clip, transform.position);
    }

    public void Die()
    {
        if (deathSound) AudioSource.PlayClipAtPoint(deathSound.clip, transform.position);
        
    }
}
