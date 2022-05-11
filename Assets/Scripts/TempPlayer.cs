using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void Update()
    {
        weapon.update(Time.deltaTime);

        if (Input.GetMouseButton(0))
            weapon.Pressed();
        else
            weapon.Released();

        if (weapon is Gun && Input.GetKeyDown(KeyCode.R))
            (weapon as Gun).Reload();
    }
}
