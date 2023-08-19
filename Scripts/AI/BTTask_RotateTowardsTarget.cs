using UnityEngine;

public class BTTask_RotateTowardsTarget : BTNode
{
    private BehaviorTree tree;
    private GameObject target;
    private IBehaviorTreeInterface behaviorTreeInterface;
    private string targetKey;
    private float acceptableDegrees;

    public BTTask_RotateTowardsTarget(BehaviorTree tree, string targetKey, float acceptableDegrees = 10f)
    {
        this.tree = tree;
        this.targetKey = targetKey;
        this.acceptableDegrees = acceptableDegrees;
        this.behaviorTreeInterface = tree.GetBehaviorTreeInterface();
    }

    protected override NodeResult Execute()
    {
        if (tree == null || tree.BlackBoard == null || behaviorTreeInterface == null)
            return NodeResult.Failure;

        if (!tree.BlackBoard.GetBlackboardData(targetKey, out target))
            return NodeResult.Failure;

        if (IsAcceptableDegrees())
            return NodeResult.Success;

        tree.BlackBoard.onBlackboardValueChanged += BlackBoardValueChanged;

        return NodeResult.InProgress;
    }

    private void BlackBoardValueChanged(string key, object value)
    {
        if (key == targetKey)
        {
            target = (GameObject)value;
        }
    }

    protected override NodeResult Update()
    {
        if (target == null)
            return NodeResult.Failure;

        if (IsAcceptableDegrees())
            return NodeResult.Success;

        behaviorTreeInterface.RotateTowards(target);

        return NodeResult.InProgress;
    }

    private bool IsAcceptableDegrees()
    {
        if (target == null)
            return false;

        Vector3 targetDir = (target.transform.position - tree.transform.position).normalized;
        Vector3 dir = tree.transform.forward;

        float degrees = Vector3.Angle(targetDir, dir);

        return degrees <= acceptableDegrees;
    }

    protected override void End()
    {
        tree.BlackBoard.onBlackboardValueChanged -= BlackBoardValueChanged;

        base.End();
    }
}
