using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToLocation : BTNode
{
    private NavMeshAgent agent;
    private BehaviorTree tree;
    private Vector3 location;
    private string locationKey;
    private float acceptableDistance = 1f;

    public BTTask_MoveToLocation(BehaviorTree tree, string locationKey, float acceptableDistance = 1f)
    {
        this.tree = tree;
        this.locationKey = locationKey;
        this.acceptableDistance = acceptableDistance;
    }

    protected override NodeResult Execute()
    {
        BlackBoard blackBoard = tree.BlackBoard;

        if (blackBoard == null || !blackBoard.GetBlackboardData(locationKey, out location))
            return NodeResult.Failure;

        agent = tree.GetComponent<NavMeshAgent>();

        if (agent == null)
            return NodeResult.Failure;

        if (IsLocationAcceptableDistance())
            return NodeResult.Success;

        agent.SetDestination(location);
        agent.isStopped = false;

        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        if (IsLocationAcceptableDistance())
        {
            agent.isStopped = true;
            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }

    private bool IsLocationAcceptableDistance()
    {
        return Vector3.Distance(location, tree.transform.position) < acceptableDistance;
    }

    protected override void End()
    {
        agent.isStopped = true;
        base.End();
    }
}
