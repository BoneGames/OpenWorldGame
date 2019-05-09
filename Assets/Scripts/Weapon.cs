using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class Weapon : MonoBehaviour
{
    [BoxGroup("Stats")] public int damage = 10, maxAmmo = 500, maxClip = 20;
    [BoxGroup("Stats")] public float spread = 2f, recoil = 1f, range = 10f, shootRate = 0.2f;
    [BoxGroup("References")] public Transform shotOrigin;
    [BoxGroup("References")] public GameObject bulletPrefab;

    [HideInInspector] public bool canShoot = false;


    private float shootTimer = 0f;

    private int ammo = 0;


    // Update is called once per frame
    void Update()
    {
        // Count up shoot timer
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootRate)
        {
            canShoot = true;
        }
    }

    public void Reload()
    {
        //
        ammo -= maxClip;
    }
    public void Shoot()
    {
        // Reset timer & canShoot to false
        shootTimer = 0;
        canShoot = false;

        // Get some values
        Camera attachedCamera = Camera.main; // Note (Manny): Pass the reference into weapon somehow
        Transform camTransform = attachedCamera.transform; // Shortening Camera's Transform to 'camTransform'
        Vector3 bulletOrigin = camTransform.position; // Starting point of the bullet (Centre of Camera)
        Quaternion bulletRotation = camTransform.rotation; // Rotation of the bullet
        Vector3 lineOrigin = shotOrigin.position; // Where the bullet line starts
        Vector3 direction = camTransform.forward; // Forward direction of camera

        // Spawn Bullet
        GameObject clone = Instantiate(bulletPrefab, bulletOrigin, bulletRotation);
        Bullet bullet = clone.GetComponent<Bullet>();
        bullet.Fire(lineOrigin, direction);
    }

}
