using UnityEngine;
using UnityEngine.AI;

public class BTTask_MoveToTarget : BTNode
{
    private NavMeshAgent agent;
    private BehaviorTree tree;
    private GameObject target;
    private string targetKey;
    private float acceptableDistance = 1f;

    public BTTask_MoveToTarget(BehaviorTree tree, string targetKey, float acceptableDistance = 1f)
    {
        this.targetKey = targetKey;
        this.acceptableDistance = acceptableDistance;
        this.tree = tree;
    }

    protected override NodeResult Execute()
    {
        BlackBoard blackBoard = tree.BlackBoard;

        if (blackBoard == null || !blackBoard.GetBlackboardData(targetKey, out target))
            return NodeResult.Failure;

        agent = tree.GetComponent<NavMeshAgent>();

        if (agent == null)
            return NodeResult.Failure;

        if (IsTargetAcceptableDistance())
            return NodeResult.Success;

        blackBoard.onBlackboardValueChanged += BlackboardOnValueChanged;

        agent.SetDestination(target.transform.position);
        agent.isStopped = false;

        return NodeResult.InProgress;
    }

    private void BlackboardOnValueChanged(string key, object value)
    {
        if (key == targetKey)
        {
            target = (GameObject)value;
        }
    }

    protected override NodeResult Update()
    {
        if (target == null)
        {
            agent.isStopped = true;
            return NodeResult.Failure;
        }

        agent.SetDestination(target.transform.position);

        if (IsTargetAcceptableDistance())
        {
            agent.isStopped = true;
            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }

    private bool IsTargetAcceptableDistance()
    {
        return Vector3.Distance(target.transform.position, tree.transform.position) <= acceptableDistance;
    }

    protected override void End()
    {
        agent.isStopped = true;
        tree.BlackBoard.onBlackboardValueChanged -= BlackboardOnValueChanged;
        base.End();
    }
}
