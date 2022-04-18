using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    Pistol,
    MachineGun
}

public enum WeaponFiringPattern
{
    SemiAutio,
    FullAuto,
    ThreeShotBurst,
    FiveShotBurst
}

[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public string weaponName;
    public float damage;
    public int bulletsInClip;
    public int clipSize;
    public float fireStartDelay;
    public float fireRate;
    public WeaponFiringPattern weaponFiringPattern;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayers;
    public int totalBullets;
}

public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public Transform firingEffectLocation;

    protected WeaponHolder weaponHolder;
    [SerializeField]
    protected ParticleSystem firingEffect;

    [SerializeField]
    public WeaponStats weaponStats;

    public bool isFiring = false;
    public bool isReloading = false;
    protected Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        firingEffect.gameObject.transform.parent = firingEffectLocation;
    }

    public void Initialize(WeaponHolder _weaponHolder, WeaponScriptable weaponScriptable)
    {
        weaponHolder = _weaponHolder;

        if(weaponScriptable)
        {
            weaponStats = weaponScriptable.weaponStats;
        }
    }

    // decide whether it is automatic or semi-auto here
    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if(weaponStats.repeating)
        {
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));
        if(firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
        //Debug.Log("Stopped firing weapon");
    }

    protected virtual void FireWeapon()
    {
        //print("Firing Weapon!");
        weaponStats.bulletsInClip--;
    }

    // deal with ammo counts and maybe particle effects
    public virtual void StartReloading()
    {
        isReloading = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        isReloading = false;
    }

    protected virtual void ReloadWeapon()
    {
        // if there's a firing effect hide it here
        if(firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
        
        // if there's a firing effect, hide it here
        int bulletsToReload = weaponStats.totalBullets - (weaponStats.clipSize - weaponStats.bulletsInClip);
        //int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;
        
        // COD Style reload, subtract bullets
        if(bulletsToReload > 0)
        {
            weaponStats.totalBullets = bulletsToReload;
            weaponStats.bulletsInClip = weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip += weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
    }
}