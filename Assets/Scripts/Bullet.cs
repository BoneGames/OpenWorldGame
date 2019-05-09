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

    Vector3 start, end;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        start = transform.position;
    }
    void Update()
    {
        line.transform.rotation = Quaternion.LookRotation(rigid.velocity);
    }
    void OnCollisionEnter(Collision col)
    {
        end = transform.position;
        // Get contact point from collision
        ContactPoint contact = col.contacts[0];
        // Spawn a BulletHole on that contact point
        GameObject bHole = Instantiate(bulletHolePrefab, contact.point, Quaternion.LookRotation(contact.normal) *
                                                     Quaternion.AngleAxis(90, Vector3.right));

        Vector3 bulletDir = (end - start).normalized;
        bHole.transform.rotation = Quaternion.LookRotation(bulletDir);

        // the bullet is stretching in the right direction, but still needs to be oriented to be flat on the surface it lands on - is currently lying on the path the bullet took
        //bHole.transform.right = contact.point;
   
            float stretch = (180 - Vector3.Angle(bulletDir, contact.normal))/72;
            Debug.Log(stretch);
   
        bHole.transform.localScale = new Vector3(0.25f, 0.25f, stretch);

   
        //Debug.Log("contactNormal: " + contact.normal);
        //Debug.Log("bulletTrans: " + transform.forward);
        

        
        Quaternion lookRot = Quaternion.LookRotation(transform.forward, contact.normal); 
     
        //bHole.transform.localScale = new Vector3(0, 0, stretch);
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
