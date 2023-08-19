using UnityEngine;

public class BTTask_GetNextPatrolPoint : BTNode
{
    private PatrollingComponent patrollingComponent;
    private BehaviorTree tree;
    private string patrolPointKey;

    public BTTask_GetNextPatrolPoint(BehaviorTree tree, string patrolPointKey)
    {
        this.tree = tree;
        this.patrolPointKey = patrolPointKey;
        patrollingComponent = tree.GetComponent<PatrollingComponent>();
    }

    protected override NodeResult Execute()
    {
        if (patrollingComponent != null && patrollingComponent.GetNextPatrolPoint(out Vector3 point))
        {
            tree.BlackBoard.SetOrAddData(patrolPointKey, point);
            return NodeResult.Success;
        }

        return NodeResult.Failure;
    }
}
