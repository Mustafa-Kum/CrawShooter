public class BTTask_RemoveBlackboardData : BTNode
{
    private BehaviorTree tree;
    private string keyToRemove;

    public BTTask_RemoveBlackboardData(BehaviorTree tree, string keyToRemove)
    {
        this.tree = tree;
        this.keyToRemove = keyToRemove;
    }

    protected override NodeResult Execute()
    {
        if (tree != null && tree.BlackBoard != null)
        {
            tree.BlackBoard.RemoveBlackboardData(keyToRemove);
            return NodeResult.Success;
        }

        return NodeResult.Failure;
    }
}
