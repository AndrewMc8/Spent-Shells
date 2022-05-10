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

    public override List<GameObject> GenerateHits(Vector3 origin, Vector3 direction, float range)
    {
        List<GameObject> hitObjects = new List<GameObject>();

        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, range))
        {
            if (!raycastHit.collider.CompareTag(tag))
            {
                hitObjects.Add(raycastHit.collider.gameObject);
            }
        }

        return hitObjects;
    }
}
