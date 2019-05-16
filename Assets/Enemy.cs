using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class Enemy : MonoBehaviour, IHealth
{
    [ProgressBar("Health", 100, ProgressBarColor.Red)]
    public int health = 100;
    public Transform target;
    public Rigidbody rigid;
    private NavMeshAgent agent;
    
    
    public float jump;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //agent.SetDestination(target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (agent.isOnNavMesh)
        {
            if (collision.transform.tag == "Enemy")
            {
                Debug.Log("jump");
                rigid.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
            }
        }
    }
    public void Heal(int heal)
    {
        health += heal;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Take Damage");
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
