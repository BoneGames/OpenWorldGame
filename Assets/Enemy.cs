using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Rigidbody rigid;
    private NavMeshAgent agent;
    public float jump;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(agent.isOnNavMesh)
        {
            if(collision.transform.tag == "Enemy")
            {
                Debug.Log("jump");
                rigid.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
            }
        }
    }
}
