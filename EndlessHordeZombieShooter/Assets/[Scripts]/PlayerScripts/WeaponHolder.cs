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
    WeaponComponent equippedWeapon;
    
    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform gripIKSocketLocation;

    bool wasFiring = false;
    bool firingPressed = false;
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        GameObject spawnedWeapon =
        Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialize(this);
        PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
        
        gripIKSocketLocation = equippedWeapon.gripLocation;
    }

    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex) 
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocketLocation.transform.position);
    }
    
    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;

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

}