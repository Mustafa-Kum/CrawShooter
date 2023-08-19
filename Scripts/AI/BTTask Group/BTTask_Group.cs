public abstract class BTTask_Group : BTNode
{
    protected BTNode root;
    protected BehaviorTree tree;

    public BTTask_Group(BehaviorTree tree)
    {
        this.tree = tree;
    }

    protected abstract void ConstructTree(out BTNode root);

    protected override NodeResult Execute()
    {
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        return root.UpdateNode();
    }

    protected override void End()
    {
        root.Abort();
        base.End();
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);
        root.SortPriority(ref priorityCounter);
    }

    public override void Initialize()
    {
        base.Initialize();
        ConstructTree(out root);
    }

    public override BTNode Get()
    {
        return root.Get();
    }
}
