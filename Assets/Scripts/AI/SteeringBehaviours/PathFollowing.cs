using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "PathFollowing", menuName = "SteeringBehavious/PathFollowing", order = 1)]
public class PathFollowing : SteeringBehaviour
{
    public float nodeRadius = .1f, targetRadius = 3f;
    private int currentNode = 0;
    private bool isAtTarget = false;

    private NavMeshPath path;

    public override void OnDrawGizmosSelected(AI owner)
    {
        if (path != null)
        {
            Vector3[] points = path.corners;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Vector3 pointA = points[i];
                Vector3 pointB = points[i + 1];
                // Draw a line between both points
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(pointA, pointB);
            }
        }
    }

    public override Vector3 GetForce(AI owner)
    {
        Vector3 force = Vector3.zero;

        NavMeshAgent agent = owner.agent;

        if (owner.hasTarget)
        {
            path = new NavMeshPath();
            // Get path to target
            if (agent.CalculatePath(owner.target.position, path))
            {
                // Check if path has completed calculating
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    Vector3[] points = path.corners;
                    // If there are points in the path
                    if (points.Length > 0)
                    {
                        // Get last node in array
                        int lastNode = points.Length - 1;
                        // Select the minimum value of the two values
                        currentNode = Mathf.Min(currentNode, lastNode);
                        // Get the current point
                        Vector3 currentPoint = points[currentNode];
                        // Check if it is the last node
                        isAtTarget = currentNode == lastNode;
                        // Get distance to current point
                        float distanceToNode = Vector3.Distance(owner.transform.position, currentPoint);
                        // If the distance between AI and node is less than nodeRadius
                        if (distanceToNode < nodeRadius)
                        {
                            // Go to next node
                            currentNode++;
                        }
                        // Set force direction to current point;
                        force = currentPoint - owner.transform.position;
                    }
                }
            }
        }

        return force.normalized;
    }
}
