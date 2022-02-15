using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public delegate void OnWeaponEquippedEvent(WeaponComponent weaponComponent);

    public static event OnWeaponEquippedEvent onWeaponEquipped;

    public static void InvokeOnWeaponEquipped(WeaponComponent weaponComponent)
    {
        onWeaponEquipped?.Invoke(weaponComponent);
    }
}
