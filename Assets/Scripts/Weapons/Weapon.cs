using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Weapon : MonoBehaviour
{
    [BoxGroup("Stats")] public int damage = 10;
    [BoxGroup("Stats")] public float attackRate = 5f;

    [HideInInspector] public bool canShoot = false;

    private float attackTimer = 0f;

    void Update()
    {
        // Count up shoot timer
        attackTimer += Time.deltaTime;
        // If shoot timer reaches shoot rate
        if (attackTimer >= 1f / attackRate)
        {
            // Can shoot!
            canShoot = true;
        }
    }

    public virtual void Attack()
    {
        // Reset attack timer
        attackTimer = 0f;
        // Reset can shoot
        canShoot = false;
    }
}
