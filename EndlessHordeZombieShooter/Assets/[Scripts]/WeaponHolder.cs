using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    GameObject weaponToSpawn;

    PlayerController playerController;
    Sprite crosshairImage;
    
    [SerializeField]
    GameObject weaponSocketLocation;

    void Start()
    {
        GameObject spawnedWeapon =
        Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
    }

    void Update()
    {
        
    }
}
