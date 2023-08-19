using UnityEngine;

public class SightSense : SenseComponent
{
    [Header("Sense Info")]
    [Space]
    [SerializeField] private float sightDistance = 5f;
    [SerializeField] private float sightHalfAngle = 5f;
    [SerializeField] private float eyeHeight = 1f;

    protected override bool IsPerceptionSensable(Perception perception)
    {
        float distance = Vector3.Distance(perception.transform.position, transform.position);

        if (distance > sightDistance)
            return false;

        Vector3 forwardDir = transform.forward;
        Vector3 perceptionDir = (perception.transform.position - transform.position).normalized;

        if (Vector3.Angle(forwardDir, perceptionDir) > sightHalfAngle)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up * eyeHeight, perceptionDir, out RaycastHit hitInfo, sightDistance))
        {
            if (hitInfo.collider.gameObject != perception.gameObject)
            {
                return false;
            }
        }

        return true;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();

        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        Vector3 leftLimitDir = Quaternion.AngleAxis(sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(-sightHalfAngle, Vector3.up) * transform.forward;

        Gizmos.DrawWireSphere(drawCenter, sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + leftLimitDir * sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * sightDistance);
    }
}
