using System.Collections.Generic;

public abstract class Compositor : BTNode
{
    private LinkedList<BTNode> children = new LinkedList<BTNode>();
    private LinkedListNode<BTNode> currentChildNode = null;

    public void AddChild(BTNode newChild)
    {
        children.AddLast(newChild);
    }

    protected override NodeResult Execute()
    {
        if (children.Count == 0)
            return NodeResult.Success;

        currentChildNode = children.First;

        return NodeResult.InProgress;
    }

    protected BTNode GetCurrentChild()
    {
        return currentChildNode.Value;
    }

    protected bool Next()
    {
        if (currentChildNode != children.Last)
        {
            currentChildNode = currentChildNode.Next;
            return true;
        }

        return false;
    }

    protected override void End()
    {
        if (currentChildNode == null)
            return;

        currentChildNode.Value.Abort();
        currentChildNode = null;
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);

        foreach (var child in children)
        {
            child.SortPriority(ref priorityCounter);
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        foreach (var child in children)
        {
            child.Initialize();
        }
    }

    public override BTNode Get()
    {
        if (currentChildNode == null)
        {
            if (children.Count != 0)
                return children.First.Value.Get();
            else
                return this;
        }

        return currentChildNode.Value.Get();
    }
}
