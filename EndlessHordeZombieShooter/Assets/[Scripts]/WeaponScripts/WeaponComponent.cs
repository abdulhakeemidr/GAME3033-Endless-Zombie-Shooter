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

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
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
        if(firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }

        int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;
        if(bulletsToReload < 0)
        {
            weaponStats.bulletsInClip = weaponStats.clipSize;
            weaponStats.totalBullets -= weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
    }
}