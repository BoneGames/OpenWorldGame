using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public bool hasTarget;
    [ShowIf("hasTarget")] public Transform target;
    public float maxVelocity = 15f;
    public float maxDistance = 10f;
    [Expandable]
    public SteeringBehaviour[] behaviours;
    [ReadOnly] public Vector3 velocity;
    public NavMeshAgent agent;

    public List<float> behaviourWeights = new List<float>();
    List<float> oldWeights = new List<float>();
    int? changedWeightIndex;

    //public List<float> BehaviourWeights
    //{
    //    get
    //    {
    //        return behaviourWeights;
    //    }
    //    set
    //    {
    //        Debug.Log("property accessed");
    //        changedWeightIndex = GetChangedWeightIndex(value);
    //        behaviourWeights = value;
    //    }
    //}

    int? GetChangedWeightIndex()
    {
        if(oldWeights.Count == 0)
        {
            oldWeights = behaviourWeights;
        }
        for (int i = 0; i < behaviourWeights.Count; i++)
        {
            //if(newWeights[i] == behaviourWeights[i])
            //{
            //    Debug.Log(i);
            //    return i;
            //}
        }
       
        return null;
    }

    private void OnValidate()
    {
        // give this AI to all steeringBehaviours
        foreach (SteeringBehaviour sb in behaviours)
        {
            //Debug.Log("AI Given to: " + sb.name);
            //sb.ai = this;
            behaviourWeights.Add(sb.weighting);
        }

        //DistributeWeighting();
    }

    void ApplyNewWeights()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].weighting = behaviourWeights[i];
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 desiredPosition = transform.position + velocity * Time.deltaTime;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(desiredPosition, .1f);

        // Render all behaviours
        foreach (var behaviour in behaviours)
        {
            behaviour.OnDrawGizmosSelected(this);
        }
    }

        private void Update()
    {
        Vector3 force = Vector3.zero;

        // Step 1). Loop through all behaviours and get forces
        foreach (var behaviour in behaviours)
        {
            // Apply normalized force to force
            float percentage = maxVelocity * behaviour.weighting;
            force += behaviour.GetForce(this) * percentage;
        }

        // Step 2). Apply force to velocity
        velocity += force;

        // Step 3). Limit velocity to max velocity
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        // Step 4). Apply velocity to NavMeshAgent destination
        Vector3 desiredPosition = transform.position + velocity * Time.deltaTime;
        NavMeshHit hit;
        // Check if desired position is within NavMesh
        if (NavMesh.SamplePosition(desiredPosition, out hit, maxDistance, -1))
        {
            // Set agent's destination to hit point
            agent.SetDestination(hit.position);
        }
    }
    float GetExcessWeight()
    {
        float totalWeight = 0;
        foreach (SteeringBehaviour b in behaviours)
        {
            totalWeight += b.weighting;
            Debug.Log("added " + b.weighting + " from: " + b.name);
        }
        return totalWeight - 1;
    }


    public void DistributeWeighting()
    {
        List<SteeringBehaviour> outOfRange = new List<SteeringBehaviour>();
        // get change in weight
        float excessWeight = GetExcessWeight(); // NOTE - access slider maximum in steeringBehaviour?

        if (excessWeight != 0)
        {
            float weightChangeToApply = excessWeight / (behaviours.Length - (1 + outOfRange.Count) * -1);
            Debug.Log(weightChangeToApply);

            // Add portioned weight change to all elements except the original changed one
            while (GetExcessWeight() != 0)
            {
                weightChangeToApply = excessWeight / (behaviours.Length - (1 + outOfRange.Count) * -1);
                Debug.Log(weightChangeToApply);

                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (i != changedWeightIndex)
                    {
                        if (behaviours[i].weighting <= 0)
                        {
                            behaviours[i].weighting = 0;
                            excessWeight += weightChangeToApply;
                            outOfRange.Add(behaviours[i]);
                        }
                        else if (behaviours[i].weighting >= 1)
                        {
                            behaviours[i].weighting = 1;
                            excessWeight += weightChangeToApply;
                            outOfRange.Add(behaviours[i]);
                        }
                        else
                        {
                            behaviours[i].weighting += weightChangeToApply;
                            Debug.Log("added " + weightChangeToApply + " to: " + behaviours[i].name + ". Now " + behaviours[i].name + " weighting = " + behaviours[i].weighting);
                        }
                    }
                }
            }
           

            //// add portioned weight change
            //for (int i = 0; i < behaviours.Length; i++)
            //{
            //    if (i != changedWeightIndex)
            //    {
            //        if (behaviours[i].weighting <= 0)
            //        {
            //            behaviours[i].weighting = 0;
            //            leftovers += weightChangeToApply;
            //            outOfRange.Add(behaviours[i]);
            //        }
            //        if (behaviours[i].weighting >= 1)
            //        {
            //            behaviours[i].weighting = 1;
            //            leftovers += weightChangeToApply;
            //            outOfRange.Add(behaviours[i]);
            //        }
            //        behaviours[i].weighting += weightChangeToApply;
            //        Debug.Log("added " + weightChangeToApply + " to: " + behaviours[i].name + ". Now " + behaviours[i].name + " weighting = " + behaviours[i].weighting);
            //    }
            //}
            //weightChangeToApply = leftovers / ((behaviours.Length - 1) - outOfRange.Count);

           
        }

    }
}
