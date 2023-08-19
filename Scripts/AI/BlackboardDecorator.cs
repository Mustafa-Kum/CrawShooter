public class BlackboardDecorator : Decorator
{
    public enum RunCondition
    {
        KeyExist, KeyNotExist
    }

    public enum NotifyRule
    {
        RunConditionChange, KeyValueChange
    }

    public enum NotifyAbort
    {
        None, Self, Lower, Both
    }

    private RunCondition runCondition;
    private NotifyRule notifyRule;
    private NotifyAbort notifyAbort;

    private BehaviorTree tree;

    private string key;

    private object value;

    public BlackboardDecorator(BehaviorTree tree, BTNode child, string key, RunCondition runCondition, NotifyRule notifyRule, NotifyAbort notifyAbort) : base(child)
    {
        this.tree = tree;
        this.key = key;
        this.runCondition = runCondition;
        this.notifyRule = notifyRule;
        this.notifyAbort = notifyAbort;
    }

    protected override NodeResult Execute()
    {
        BlackBoard blackBoard = tree.BlackBoard;

        if (blackBoard == null)
            return NodeResult.Failure;

        blackBoard.onBlackboardValueChanged -= CheckNotify;
        blackBoard.onBlackboardValueChanged += CheckNotify;

        if (CheckRunCondition())
            return NodeResult.InProgress;
        else
            return NodeResult.Failure;
    }

    private bool CheckRunCondition()
    {
        bool exist = tree.BlackBoard.GetBlackboardData(key, out value);

        switch (runCondition)
        {
            case RunCondition.KeyExist:
                return exist;

            case RunCondition.KeyNotExist:
                return !exist;
        }

        return false;
    }

    private void CheckNotify(string key, object val)
    {
        if (this.key != key)
            return;

        if (notifyRule == NotifyRule.RunConditionChange)
        {
            bool prevExist = value != null;
            bool currentExists = val != null;

            if (prevExist != currentExists)
            {
                Notify();
            }
        }
        else if (notifyRule == NotifyRule.KeyValueChange)
        {
            if (value != val)
            {
                Notify();
            }
        }
    }

    private void Notify()
    {
        switch (notifyAbort)
        {
            case NotifyAbort.None:
                break;

            case NotifyAbort.Self:
                AbortSelf();
                break;

            case NotifyAbort.Lower:
                AbortLower();
                break;

            case NotifyAbort.Both:
                AbortBoth();
                break;
        }
    }

    private void AbortBoth()
    {
        Abort();
        AbortLower();
    }

    private void AbortLower()
    {
        tree.AbortLowerThan(GetPriority());
    }

    private void AbortSelf()
    {
        Abort();
    }

    protected override NodeResult Update()
    {
        return GetChild().UpdateNode();
    }

    protected override void End()
    {
        GetChild().Abort();

        base.End();
    }
}
