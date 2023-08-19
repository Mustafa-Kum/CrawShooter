using UnityEngine;

public class SpitterBehavior : BehaviorTree
{
    [SerializeField] private float stopDistance;
    [SerializeField] private float rotateRadiusLenght;
    [SerializeField] private float distanceToLocation;
    [SerializeField] private float attackCooldown;

    protected override void ConstructTree(out BTNode rootNode)
    {
        Selector rootSelector = new Selector();

        rootSelector.AddChild(new BTTaskGroup_AttackTarget(this, stopDistance, rotateRadiusLenght, attackCooldown));
        rootSelector.AddChild(new BTTaskGroup_MoveToLastSeenLocation(this, distanceToLocation));
        rootSelector.AddChild(new BTTaskGroup_Patrolling(this, distanceToLocation));

        rootNode = rootSelector;
    }
}
