public class BTTaskGroup_AttackTarget : BTTask_Group
{
    private float stopDistance;
    private float rotateRadius;
    private float attackCooldown;

    public BTTaskGroup_AttackTarget(BehaviorTree tree, float stopDistance = 1.3f, float rotateRadius = 10f, float attackCooldown = 0) : base(tree)
    {
        this.stopDistance = stopDistance;
        this.rotateRadius = rotateRadius;
        this.attackCooldown = attackCooldown;
    }

    protected override void ConstructTree(out BTNode root)
    {
        Sequencer attackTargetSequencer = new Sequencer();
        BTTask_MoveToTarget moveToTarget = new BTTask_MoveToTarget(tree, "Target", stopDistance);
        BTTask_RotateTowardsTarget rotateTowardsTarget = new BTTask_RotateTowardsTarget(tree, "Target", rotateRadius);
        BTTask_AttackTarget attackTarget = new BTTask_AttackTarget(tree, "Target");
        CooldownDecorator attackCooldownDecorator = new CooldownDecorator(tree, attackTarget, attackCooldown);

        attackTargetSequencer.AddChild(moveToTarget);
        attackTargetSequencer.AddChild(rotateTowardsTarget);
        attackTargetSequencer.AddChild(attackCooldownDecorator);

        BlackboardDecorator attackTargetDecorator = new BlackboardDecorator(tree, attackTargetSequencer, "Target",
                                                                            BlackboardDecorator.RunCondition.KeyExist,
                                                                            BlackboardDecorator.NotifyRule.RunConditionChange,
                                                                            BlackboardDecorator.NotifyAbort.Both);

        root = attackTargetDecorator;
    }
}
