public class BTTaskGroup_MoveToLastSeenLocation : BTTask_Group
{
    private float distance;

    public BTTaskGroup_MoveToLastSeenLocation(BehaviorTree tree, float distance = 2) : base(tree)
    {
        this.distance = distance;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer checkLastSeenLocationSequence = new Sequencer();
        BTTask_MoveToLocation moveToLastSeenLocation = new BTTask_MoveToLocation(tree, "LastSeenLocation", distance);
        BTTask_Wait waitAtLastSeenLocation = new BTTask_Wait(2f);
        BTTask_RemoveBlackboardData removeLastSeenLocation = new BTTask_RemoveBlackboardData(tree, "LastSeenLocation");
        
        checkLastSeenLocationSequence.AddChild(moveToLastSeenLocation);
        checkLastSeenLocationSequence.AddChild(waitAtLastSeenLocation);
        checkLastSeenLocationSequence.AddChild(removeLastSeenLocation);


        BlackboardDecorator checkLastSeenLocationDecorator = new BlackboardDecorator(tree, checkLastSeenLocationSequence, "LastSeenLocation",
                                                                                     BlackboardDecorator.RunCondition.KeyExist,
                                                                                     BlackboardDecorator.NotifyRule.RunConditionChange,
                                                                                     BlackboardDecorator.NotifyAbort.None);

        root = checkLastSeenLocationDecorator;
    }
}
