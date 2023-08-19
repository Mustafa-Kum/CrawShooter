public class BTTaskGroup_Patrolling : BTTask_Group
{
    private float distance;

    public BTTaskGroup_Patrolling(BehaviorTree tree, float distance = 1f) : base(tree)
    {
        this.distance = distance;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer patrollingSequence = new Sequencer();
        BTTask_GetNextPatrolPoint getNextPatrolPoint = new BTTask_GetNextPatrolPoint(tree, "PatrolPoint");
        BTTask_MoveToLocation moveToPatrolPoint = new BTTask_MoveToLocation(tree, "PatrolPoint", distance);
        BTTask_Wait waitAtPatrolPoint = new BTTask_Wait(2f);

        patrollingSequence.AddChild(getNextPatrolPoint);
        patrollingSequence.AddChild(moveToPatrolPoint);
        patrollingSequence.AddChild(waitAtPatrolPoint);

        root = patrollingSequence;
    }
}
