using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckShot : BulletLogic
{
    [SerializeField] private int pelletCount = 1;
    [SerializeField] private Vector2 stdPelletDeviation = Vector2.zero;

    protected void OnValidate()
    {
        if(gameObject.transform.TryGetComponent<Gun>(out Gun gun))
        {
            gun.Validate();
        }

        if (pelletCount < 1)
            pelletCount = 1;

        if (stdPelletDeviation.x < 0 || stdPelletDeviation.y < 0)
            stdPelletDeviation = new Vector2(Mathf.Abs(stdPelletDeviation.x), Mathf.Abs(stdPelletDeviation.y));
    }

    public override List<GameObject> GenerateHits(Vector3 origin, Vector3 direction, float range)
    {
        List<GameObject> hitObjects = new List<GameObject>();

        for(int i = 0; i < pelletCount; i++)
        {
            Vector3 deviatedDirection = Vector3.zero;

            deviatedDirection.x = Random.Range(-stdPelletDeviation.x, stdPelletDeviation.x);
            deviatedDirection.y = Random.Range(-stdPelletDeviation.y, stdPelletDeviation.y);

            if (i == 0)
                deviatedDirection = Vector3.zero;

            deviatedDirection = direction + deviatedDirection;

            Ray ray = new Ray(origin, deviatedDirection);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, range))
            {
                if (!raycastHit.collider.CompareTag(tag))
                {
                    hitObjects.Add(raycastHit.collider.gameObject);
                }
            }
        }


        return hitObjects;
    }
}
