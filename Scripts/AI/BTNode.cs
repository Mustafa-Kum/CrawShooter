public enum NodeResult
{
    Success, Failure, InProgress
}

public abstract class BTNode
{
    private bool started = false;
    private int priority;

    public NodeResult UpdateNode()
    {
        if (!started)
        {
            started = true;

            NodeResult execResult = Execute();

            if (execResult != NodeResult.InProgress)
            {
                EndNode();

                return execResult;
            }
        }

        NodeResult updateResult = Update();

        if (updateResult != NodeResult.InProgress)
            EndNode();

        return updateResult;
    }

    protected virtual NodeResult Execute()
    {
        return NodeResult.Success;
    }

    protected virtual NodeResult Update()
    {
        return NodeResult.Success;
    }

    protected virtual void End()
    {
        // Only override.
    }

    private void EndNode()
    {
        started = false;
        End();
    }

    public void Abort()
    {
        EndNode();
    }

    public int GetPriority()
    {
        return priority;
    }

    public virtual void SortPriority(ref int priorityCounter)
    {
        priority = priorityCounter++;
    }

    public virtual void Initialize()
    {

    }

    public virtual BTNode Get()
    {
        return this;
    }
}
