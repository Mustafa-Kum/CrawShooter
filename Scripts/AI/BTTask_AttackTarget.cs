using UnityEngine;

public class BTTask_AttackTarget : BTNode
{
    private BehaviorTree tree;
    private GameObject target;
    private string targetKey;

    public BTTask_AttackTarget(BehaviorTree tree, string targetKey)
    {
        this.tree = tree;
        this.targetKey = targetKey;
    }

    protected override NodeResult Execute()
    {
        if (!tree || tree.BlackBoard == null || !tree.BlackBoard.GetBlackboardData(targetKey, out target))
            return NodeResult.Failure;

        IBehaviorTreeInterface behaviorTreeInterface = tree.GetBehaviorTreeInterface();

        if (behaviorTreeInterface == null)
            return NodeResult.Failure;

        behaviorTreeInterface.AttackTarget(target);

        return NodeResult.Success;
    }
}
