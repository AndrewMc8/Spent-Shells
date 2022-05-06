using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] protected float weight = 0;
    [SerializeField] protected float value = 0;
    [SerializeField] protected new string name;

    [HideInInspector] public float Weight { get { return weight; } }
    [HideInInspector] public float Value { get { return value; } }
    [HideInInspector] public string Name { get { return name; } }

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(this.name) || string.IsNullOrEmpty(this.name)) this.name = gameObject.name;
    }

    public abstract string GetDesc();

    public abstract System.Type GetBaseType();
}
