using System.Collections.Generic;
using UnityEngine;

public abstract class BulletLogic : MonoBehaviour
{
    public abstract List<GameObject> GenerateHits(Transform origin, Vector3 baseDirection, float range);
}
