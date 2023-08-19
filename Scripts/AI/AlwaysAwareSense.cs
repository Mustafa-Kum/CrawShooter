using UnityEngine;

public class AlwaysAwareSense : SenseComponent
{
    [SerializeField] private float awareDistance = 2f;

    protected override bool IsPerceptionSensable(Perception perception)
    {
        return Vector3.Distance(transform.position, perception.transform.position) <= awareDistance;
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();
        Gizmos.DrawWireSphere(transform.position + Vector3.up, awareDistance);
    }
}
