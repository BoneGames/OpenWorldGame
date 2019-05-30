using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;


public abstract class SteeringBehaviour : ScriptableObject
{
    public AI ai;
    [Slider(0f, 1f)]
    public float weighting = 1f;
    public abstract Vector3 GetForce(AI owner);

    public virtual void OnDrawGizmosSelected(AI ai) { }

    //public abstract void OnValidate();
}