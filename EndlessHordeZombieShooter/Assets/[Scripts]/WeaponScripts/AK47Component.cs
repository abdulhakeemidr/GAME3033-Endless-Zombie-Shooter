using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47Component : WeaponComponent
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void FireWeapon()
    {
        Vector3 hitLocation;

        if(weaponStats.bulletsInClip > 0 && !isReloading 
            && !weaponHolder.playerController.isRunning)
        {
            base.FireWeapon();
            
            if(firingEffect)
            {
                firingEffect.Play();
            }

            Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            
            if(Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.weaponHitLayers))
            {
                hitLocation = hit.point;

                Vector3 hitDirection = hit.point - mainCamera.transform.position;
                Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1);
            }
            print("Bullet count: " + weaponStats.bulletsInClip);
            
        }
        else if(weaponStats.bulletsInClip <= 0)
        {
            weaponHolder.StartReloading();
        }
    }
}
