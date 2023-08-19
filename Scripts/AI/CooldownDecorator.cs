using UnityEngine;

public class CooldownDecorator : Decorator
{
    private float cooldown;
    private float lastExecutionTime = -1;
    private bool failOnCooldown;

    public CooldownDecorator(BehaviorTree tree, BTNode child, float cooldown, bool failOnCooldown = false) : base(child)
    {
        this.cooldown = cooldown;
        this.failOnCooldown = failOnCooldown;
    }

    protected override NodeResult Execute() // If there is a problem turn back.
    {
        if (cooldown <= 0)
        {
            return NodeResult.InProgress;
        }

        if (lastExecutionTime == -1)
        {
            lastExecutionTime = Time.timeSinceLevelLoad;
            return NodeResult.InProgress;
        }

        if (Time.timeSinceLevelLoad - lastExecutionTime < cooldown)
        {
            return failOnCooldown ? NodeResult.Failure : NodeResult.Success;
        }

        lastExecutionTime = Time.timeSinceLevelLoad;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        return GetChild().UpdateNode();
    }
}
