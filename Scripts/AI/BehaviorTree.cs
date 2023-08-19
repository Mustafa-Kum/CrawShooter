using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private BTNode root;
    private BlackBoard blackBoard = new BlackBoard();
    private IBehaviorTreeInterface behaviourTreeInterface;
    public BlackBoard BlackBoard { get { return blackBoard; } }

    void Start()
    {
        behaviourTreeInterface = GetComponent<IBehaviorTreeInterface>();
        ConstructTree(out root);
        SortTree();
    }

    private void SortTree()
    {
        int priorityCounter = 0;
        root.Initialize();
        root.SortPriority(ref priorityCounter);
    }

    protected abstract void ConstructTree(out BTNode rootNode);

    void Update()
    {
        root.UpdateNode();
    }

    public void AbortLowerThan(int priority)
    {
        BTNode currentNode = root.Get();

        if (currentNode.GetPriority() > priority)
            root.Abort();
    }

    internal IBehaviorTreeInterface GetBehaviorTreeInterface() => behaviourTreeInterface;
}
