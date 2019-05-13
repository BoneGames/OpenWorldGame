using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    [BoxGroup("References")] public GameObject bulletHolePrefab;
    [BoxGroup("References")] public Transform line;
    bool hit = false;

    private Rigidbody rigid;

    public GameObject cube;

    Vector3 start, end;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        start = transform.position;
    }


    public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis)
    {
        Vector3 right = Vector3.Cross(axis, forward);
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }

    // Sorting method to get largest float magnitude in normal Vector NOTE only works well for surfaces oriented to world axis
    Vector3 Displacement(Vector3 _displacement, Vector3 normal)
    {
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
        {
            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                _displacement = new Vector3(_displacement.x, 0, 0);
            }
        }
        else if (Mathf.Abs(normal.z) > Mathf.Abs(normal.x))
        {
            if (Mathf.Abs(normal.z) > Mathf.Abs(normal.y))
            {
                _displacement = new Vector3(0, 0, _displacement.z);
            }
        }
        else
        {
            _displacement = new Vector3(0, _displacement.y, 0);
        }

        return _displacement;
    }

    void OnCollisionEnter(Collision col)
    {
        // Get contact point from collision
        ContactPoint contact = col.contacts[0];
        // get end point Vector
        end = contact.point;
        // Get bulletDirection
        Vector3 bulletDir = end - start;
        // Get Initial rotation of bullet Decal - adding 90 for object setup;
        Quaternion rotation = Quaternion.LookRotation(contact.normal) *
                              Quaternion.AngleAxis(90, Vector3.right);

        // Spawn a BulletHole on that contact point
        GameObject bHole = Instantiate(bulletHolePrefab, contact.point, rotation); 

        // Get bullet Direction
        Vector3 dir = (end - start);

        // Get displacement on normal axis between shootpoint and impactpoint
        Vector3 displacement = Displacement(-dir, contact.normal);  
        
        // create lookPoint for bullet hole once it's flat on the face it's hit
        Vector3 lookPoint = (transform.position + dir) + displacement;
       
        // create this lookpoint for testing purposes
        GameObject _bHole = Instantiate(cube, lookPoint, rotation);

        // rotate Decal to look at point with normal as it's up
        bHole.transform.LookAt(lookPoint, contact.normal);

        // Get angle between normal and bullet dir
        float impactAngle = 180 - Vector3.Angle(bulletDir, contact.normal);

        // Get scale to stretch Decal
        Vector3 newScale = new Vector3(0.2f, 0.2f, (0.2f * (1 + impactAngle / 45)));

        // Set scale
        bHole.transform.localScale = newScale;

        // Destroy self
        Destroy(gameObject);
    }
    public void Fire(Vector3 lineOrigin, Vector3 direction)
    {
        // Set line position to origin
        line.transform.position = lineOrigin;
        // Set bullet flying in direction with speed
        rigid.AddForce(direction * speed, ForceMode.Impulse);
    }
}