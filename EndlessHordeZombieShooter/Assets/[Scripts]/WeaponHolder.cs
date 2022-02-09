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
        if(equippedWeapon.weaponStats.bulletsInClip <= 0) return;

        animator.SetBool(isFiringHash, true);
        playerController.isFiring = true;

        equippedWeapon.StartFiringWeapon();
    }

    public void StopFiring()
    {
        animator.SetBool(isFiringHash, false);
        playerController.isFiring = false;

        equippedWeapon.StopFiringWeapon();
    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        animator.SetBool(isReloadingHash, playerController.isReloading);
    }

    public void StartReloading()
    {
        
    }
}