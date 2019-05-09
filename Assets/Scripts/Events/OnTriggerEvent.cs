using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent :Player
{
    public string hitTag;
    public UnityEvent onEnter, onStay, onExit;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col)
        {
            // Forces the attached collider to be a trigger
            col.isTrigger = true;
        }
        else
        {
            // Debug 
            Debug.LogWarning("GameObject: " + name + " doesn't have a collider");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == hitTag || hitTag == "")
        {
            onEnter.Invoke();
        }
        if(other.tag == "Player")
        {
            Debug.Log("aye");
            Quaternion padDir = transform.rotation;
            BoostDirectionPad(padDir);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == hitTag || hitTag == "")
        {
            onStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == hitTag || hitTag == "")
        {
            onExit.Invoke();
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
