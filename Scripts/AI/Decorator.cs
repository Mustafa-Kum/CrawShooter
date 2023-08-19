public abstract class Decorator : BTNode
{
    private BTNode child;

    protected BTNode GetChild()
    {
        return child;
    }

    public Decorator(BTNode child)
    {
        this.child = child;
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);
        child.SortPriority(ref priorityCounter);
    }

    public override void Initialize()
    {
        base.Initialize();
        child.Initialize();
    }

    public override BTNode Get()
    {
        return child.Get();
    }
}
