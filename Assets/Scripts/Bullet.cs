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

    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        line.transform.rotation = Quaternion.LookRotation(rigid.velocity);
    }
    void OnCollisionEnter(Collision col)
    {
        // Get contact point from collision
        ContactPoint contact = col.contacts[0];
        // Spawn a BulletHole on that contact point
        GameObject bHole = Instantiate(bulletHolePrefab, contact.point, Quaternion.LookRotation(contact.normal) *
                                                     Quaternion.AngleAxis(90, Vector3.right));
        Debug.Log("contactNormal: " + contact.normal);
        Debug.Log("bulletTrans: " + transform.forward);

      //bHole.transform.localScale = ).normalized);
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
