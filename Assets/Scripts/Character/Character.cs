using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Health health;
    public bool die = false;
    public bool dead = false;

    public void update(float dt)
    {
        if(!die)
        {
            health?.update(Time.deltaTime);
        }

        if (die && !dead)
        {
            dead = true;
            Die();
        }
    }


    public void WeakPointHit(Collider collider)
    {

    }

    public abstract void Die();
}
