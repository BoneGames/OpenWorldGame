using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;


[RequireComponent(typeof(Player))]
public class Combat : MonoBehaviour, IHealth
{
    [ProgressBar("Health", 100, ProgressBarColor.Green)]
    public int health = 100;

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
                    currentWeapon.Attack();
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("Ya dead!");
        }
    }

    public void Heal(int heal)
    {
        health += heal;
    }
}
