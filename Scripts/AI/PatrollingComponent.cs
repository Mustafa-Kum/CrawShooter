using UnityEngine;

public class PatrollingComponent : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;

    private int currentPatrolPointIndex = -1;

    public bool GetNextPatrolPoint(out Vector3 nextPoint)
    {
        nextPoint = Vector3.zero;

        if (patrolPoints.Length == 0)
            return false;

        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;

        nextPoint = patrolPoints[currentPatrolPointIndex].position;

        return true;
    }
}
