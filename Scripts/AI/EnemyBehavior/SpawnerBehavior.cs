using UnityEngine;

public class SpawnerBehavior : BehaviorTree
{
    [SerializeField] private float _spawnCooldown;

    protected override void ConstructTree(out BTNode rootNode)
    {
        BTTask_Spawn spawnTask = new BTTask_Spawn(this);
        CooldownDecorator spawnCooldown = new CooldownDecorator(this, spawnTask, _spawnCooldown);
        BlackboardDecorator spawnDecorator = new BlackboardDecorator(this, spawnCooldown, "Target",
                                                                     BlackboardDecorator.RunCondition.KeyExist,
                                                                     BlackboardDecorator.NotifyRule.RunConditionChange,
                                                                     BlackboardDecorator.NotifyAbort.Both);

        rootNode = spawnDecorator;
    }
}
