using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Projectiles.Effects;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : Projectile
    {
        public float speed = 50f;
        [BoxGroup("References")] public GameObject bulletHolePrefab;
        [BoxGroup("References")] public Transform line;

        private Rigidbody rigid;
        public GameObject cube;

        Vector3 start, end;

        Vector3 startL, endL;


        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }
        void Start()
        {
            start = transform.position;
        }

        public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis)
        {
            Vector3 right = Vector3.Cross(axis, forward);
            return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
        }

        // Sorting method to get largest float magnitude in normal Vector NOTE only works well for surfaces oriented to world axis
        Vector3 GetLookPoint(float bulletDist, Vector3 normal, float hitAngle, Vector3 bulletDir, Vector3 hitPoint)
        {
            float normalUnits = Mathf.Abs(Mathf.Cos(hitAngle) * bulletDist);
            //Debug.Log(normal);
            Vector3 lookPoint = hitPoint + bulletDir + (normal * normalUnits/(Mathf.Abs(normal.x) + Mathf.Abs(normal.y) + Mathf.Abs(normal.z)));
            startL = hitPoint + bulletDir;
            endL = lookPoint;
            Debug.DrawLine(startL, endL, Color.red, 5);
            return lookPoint;
            

            //if (Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
            //{
            //    if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            //    {
            //        _displacement = new Vector3(_displacement.x, 0, 0);
            //    }
            //}
            //else if (Mathf.Abs(normal.z) > Mathf.Abs(normal.x))
            //{
            //    if (Mathf.Abs(normal.z) > Mathf.Abs(normal.y))
            //    {
            //        _displacement = new Vector3(0, 0, _displacement.z);
            //    }
            //}
            //else
            //{
            //    _displacement = new Vector3(0, _displacement.y, 0);
            //}

            //return _displacement;
        }

     
        void OnCollisionEnter(Collision col)
        {
            // Manny change prefab version
            if (Player.Manny)
            {
                // Get contact point from collision
                ContactPoint contact = col.contacts[0];
                // get end point Vector
                end = contact.point;
                // Get bulletDirection
                Vector3 bulletDir = end - start;

                Quaternion lookRotation = Quaternion.LookRotation(bulletDir);

                Quaternion rotation = lookRotation * Quaternion.AngleAxis(-90, Vector3.right);

                // Spawn a BulletHole on that contact point
                GameObject clone = Instantiate(bulletHolePrefab, contact.point, rotation);

                // Get angle between normal and bullet dir
                float impactAngle = 180 - Vector3.Angle(bulletDir, contact.normal);

                clone.transform.localScale = clone.transform.localScale / (1 + impactAngle / 45);

                // Effect script
                Effect effect = clone.GetComponent<Effect>();
                effect.damage += damage;
                effect.hitObject = col.transform;

                // Destroy self
                Destroy(gameObject);
            }
            else
            {
                //Get contact point from collision
                ContactPoint contact = col.contacts[0];
                // get end point Vector
                end = contact.point;
                // Get bulletDirection
                Vector3 bulletDir = end - start;
                // Get Initial rotation of bullet Decal - adding 90 for object setup;
                Quaternion rotation = Quaternion.LookRotation(contact.normal) *
                                      Quaternion.AngleAxis(90, Vector3.right);

                // Spawn a BulletHole on that contact point
                GameObject clone = Instantiate(bulletHolePrefab, contact.point, rotation);

                float bulletDist = Vector3.Distance(start, end);

                // Get displacement on normal axis between shootpoint and impactpoint
                //Vector3 displacement = Displacement(-dir, contact.normal);

                // Get angle between normal and bullet dir
                float impactAngle = 180 - Vector3.Angle(bulletDir, contact.normal);

                // create lookPoint for bullet hole once it's flat on the face it's hit
                Vector3 lookPoint = GetLookPoint(bulletDist, contact.normal, impactAngle, bulletDir, end);

                // create this lookpoint for testing purposes
                GameObject _bHole = Instantiate(cube, lookPoint, rotation);

                // rotate Decal to look at point with normal as it's up
                clone.transform.LookAt(lookPoint, contact.normal);

                // Get scale to stretch Decal
                Vector3 newScale = new Vector3(0.2f, 0.2f, (0.2f * (1 + impactAngle / 45)));

                // Set scale
                clone.transform.localScale = newScale;

                // Effect script
                Effect effect = clone.GetComponent<Effect>();
                effect.damage += damage;
                effect.hitObject = col.transform;

                // Destroy self
                Destroy(gameObject);
            }

        }

        public override void Fire(Vector3 lineOrigin, Vector3 direction)
        {
            // Set line position to origin
            line.position = lineOrigin;
            // Set bullet flying in direction with speed
            rigid.AddForce(direction * speed, ForceMode.Impulse);
        }
    }
}