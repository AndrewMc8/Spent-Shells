using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletLogic : MonoBehaviour
{
    public abstract List<GameObject> GenerateHits(Vector3 origin, Vector3 direction, float range);
}
