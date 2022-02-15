using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponAmmoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI weaponNameText;
    [SerializeField]
    TextMeshProUGUI currentBulletCountText;
    [SerializeField]
    TextMeshProUGUI totalBulletCountText;

    [SerializeField]
    WeaponComponent weaponComponent;
    /// <summary>
    /// set up events for on weapon equipped to handle the weapon component we grab
    /// </summary>

    private void OnEnable() 
    {
        PlayerEvents.onWeaponEquipped += onWeaponEquipped;
    }

    private void OnDisable() 
    {
        PlayerEvents.onWeaponEquipped -= onWeaponEquipped;
    }

    void onWeaponEquipped(WeaponComponent _weaponComponent)
    {
        weaponComponent = _weaponComponent;
    }

    void Update()
    {
        if(!weaponComponent)
        {
            return;
        }

        weaponNameText.text = weaponComponent.weaponStats.weaponName;
        currentBulletCountText.text = weaponComponent.weaponStats.bulletsInClip.ToString();
        totalBulletCountText.text = weaponComponent.weaponStats.totalBullets.ToString();
    }
}
