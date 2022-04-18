using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    GameObject weaponToSpawn;

    public PlayerController playerController;
    Animator animator;
    Sprite crosshairImage;
    public WeaponComponent equippedWeapon;
    public WeaponAmmoUI weaponAmmoUI;
    
    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform gripIKSocketLocation;
    GameObject spawnedWeapon;
    public WeaponScriptable startingWeaponScriptable;

    bool wasFiring = false;
    bool firingPressed = false;
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        playerController.inventory.AddItem(startingWeaponScriptable, 1);
        // EquipWeapon(startingWeaponScriptable);
        // spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
        
        // startingWeaponScriptable.UseItem(playerController);
        
        // equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        // equippedWeapon.Initialize(this, startingWeaponScriptable);

        // PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
        
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        if(equippedWeapon)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocketLocation.transform.position);
        }
    }
    
    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;

        if(!equippedWeapon) return;

        if(firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }

    }

    public void StartFiring()
    {
        if(equippedWeapon.weaponStats.bulletsInClip <= 0)
        {
            StartReloading();
            return;
        }

        animator.SetBool(isFiringHash, true);
        playerController.isFiring = true;

        equippedWeapon.StartFiringWeapon();
    }

    public void StopFiring()
    {
        playerController.isFiring = false;
        animator.SetBool(isFiringHash, false);

        equippedWeapon.StopFiringWeapon();
    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;

        StartReloading();
    }

    public void StartReloading()
    {
        if(!equippedWeapon) return;

        if(equippedWeapon.isReloading || equippedWeapon.weaponStats.bulletsInClip == equippedWeapon.weaponStats.clipSize) return;
        
        if(playerController.isFiring)
        {
            StopFiring();
        }

        if(equippedWeapon.weaponStats.totalBullets <= 0) return;

        animator.SetBool(isReloadingHash, true);
        equippedWeapon.StartReloading();

        InvokeRepeating(nameof(StopReloading), 0, 0.1f);
    }

    public void StopReloading()
    {
        if(animator.GetBool(isReloadingHash)) return;

        playerController.isReloading = false;
        equippedWeapon.StopReloading();
        animator.SetBool(isReloadingHash, false);
        CancelInvoke(nameof(StopReloading));
    }

    public void EquipWeapon(WeaponScriptable weaponScriptable)
    {
        if(!weaponScriptable) return;
        spawnedWeapon =
        Instantiate(weaponScriptable.itemPrefab, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
        if(!spawnedWeapon) return;

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        if(!equippedWeapon) return;

        equippedWeapon.Initialize(this, weaponScriptable);
        PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
        gripIKSocketLocation = equippedWeapon.gripLocation;
        
        weaponAmmoUI.onWeaponEquipped(equippedWeapon);
        // do ik stuff here if other weapons are one handed, etc
        // set stuff in animator for other weapons
    }

    public void UnEquipWeapon()
    {
        if(!equippedWeapon) return;

        Destroy(equippedWeapon.gameObject);
        equippedWeapon = null;
        
    }
}