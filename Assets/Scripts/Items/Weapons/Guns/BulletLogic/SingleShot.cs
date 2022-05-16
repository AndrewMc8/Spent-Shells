using System.Collections.Generic;
using UnityEngine;

public class SingleShot : BulletLogic
{
    protected void OnValidate()
    {
        if (gameObject.transform.TryGetComponent<Gun>(out Gun gun))
        {
            gun.Validate();
        }
    }

    public override List<GameObject> GenerateHits(Transform origin, Vector3 baseDirection, float range)
    {
        List<GameObject> hitObjects = new List<GameObject>();

        Ray ray = new Ray(origin.position, baseDirection.normalized);

        Debug.DrawRay(origin.position, baseDirection.normalized * range, Color.red, 5);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, range))
        {
            if (true || !raycastHit.collider.CompareTag(tag))
            {
                hitObjects.Add(raycastHit.collider.gameObject);
            }
        }

        return hitObjects;
    }
}
