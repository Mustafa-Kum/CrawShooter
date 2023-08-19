public class BTTask_Log : BTNode
{
    private string message;

    public BTTask_Log(string message)
    {
        this.message = message;
    }

    protected override NodeResult Execute()
    {
        return NodeResult.Success;
    }
}
