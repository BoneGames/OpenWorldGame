using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wander", menuName = "SteeringBehavious/Wander", order = 1)]
public class Wander : SteeringBehaviour
{
    public float offset = 1.0f, radius = 1.0f, jitter = .2f;
    public bool freezeX = false, freezeY = true, freezeZ = false;

    private Vector3 targetDir, randomDir;

    public override void OnDrawGizmosSelected(AI owner)
    {
        Gizmos.color = Color.yellow;
        // Draw the target direction

        Vector3 ownerPosition = owner.transform.position;
        Vector3 offsetPosition = ownerPosition + targetDir.normalized * offset;
        Vector3 jitterPosition = offsetPosition + randomDir.normalized * radius;

        Gizmos.DrawLine(ownerPosition, offsetPosition);

        Gizmos.DrawWireSphere(offsetPosition, radius);
        Gizmos.DrawSphere(jitterPosition, .1f);
    }

    public override Vector3 GetForce(AI owner)
    {
        Vector3 force = Vector3.zero;

        /*
        -32767                0                     32767
        |---------------------|---------------------|
                  |_______________________|
                        Random Range
        */

        randomDir = Random.onUnitSphere;

        if (freezeX) randomDir.x = 0;
        if (freezeY) randomDir.y = 0;
        if (freezeZ) randomDir.z = 0;

        randomDir *= jitter;
        // Append target dir with random dir
        targetDir += randomDir;
        // Normalize the target dir
        targetDir = targetDir.normalized * radius;
        // Calculate seek position using targetDir
        force = targetDir + owner.transform.forward.normalized * offset;

        return force.normalized;
    }

    //public override void OnValidate()
    //{
    //     ai.DistributeWeighting(this);
    //}
}
