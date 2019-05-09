using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using NaughtyAttributes;


[RequireComponent(typeof(Player))]
public class Shooting : MonoBehaviour
{
    [BoxGroup("Weapon")] public Weapon currentWeapon;
    [BoxGroup("Weapon")] public List<Weapon> weapons = new List<Weapon>();
    [BoxGroup("Weapon")] public int currentWeaponIndex = 0;


    private Player player;
    private CameraLook cameraLook;

    void Awake()
    {
        player = GetComponent<Player>();
        cameraLook = GetComponent<CameraLook>();
    }

    void Start()
    {
        weapons = GetComponentsInChildren<Weapon>().ToList();

        SelectWeapon(0);
    }
    void DisableAllWeapons()
    {
        // loop through all game objects
        foreach (var item in weapons)
        {
            //Disable each gameobject
            item.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (currentWeapon)
        {
            bool fire1 = Input.GetButton("Fire1");
            if (fire1)
            {
                // Check if weapon can shoot
                if (currentWeapon.canShoot)
                {
                    // Shoot the weapon
                    currentWeapon.Shoot();
                    // Apply Weapon Recoil
                    Vector3 euler = Vector3.up * 2f;
                    // Randomize the pitch
                    euler.x = Random.Range(-1f, 1f);
                    // Apply offset to camera using weapon recoil
                    cameraLook.SetTargetOffset(euler * currentWeapon.recoil);
                }
            }
        }
    }

    void SelectWeapon(int index)
    {
        if(index >=0 && index < weapons.Count)
        {
            // Disable all weapons
            DisableAllWeapons();
            // Select currentWeapon
            currentWeapon = weapons[index];
            // Enable currentWeapon
            currentWeapon.gameObject.SetActive(true);
            // Update currentWeaponIndex
            currentWeaponIndex = index;
        }
    }
}
