using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : State
{
    float prevAngle;
    float prevDistance;

    public EvadeState(StateAgent owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        owner.perception.angle = 180;
        owner.perception.distance = 10;
        prevAngle = owner.perception.angle;
        prevDistance = owner.perception.distance;
    }

    public override void OnExit()
    {
        owner.perception.angle = prevAngle;
        owner.perception.distance = prevDistance;
        owner.movement.Resume();
    }

    public override void OnUpdate()
    {
        Vector3 direction = (owner.transform.position - owner.enemy.transform.position).normalized;
        owner.movement.MoveTowards(owner.transform.position + direction);
    }
}
