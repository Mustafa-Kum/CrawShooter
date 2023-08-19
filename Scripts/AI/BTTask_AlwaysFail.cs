public class BTTask_AlwaysFail : BTNode
{
    protected override NodeResult Execute()
    {
        return NodeResult.Failure;
    }
}
