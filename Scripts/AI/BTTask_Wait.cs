using UnityEngine;

public class BTTask_Wait : BTNode
{
    private float waitDuration = 2f;
    private float elapsedTime = 0f;

    public BTTask_Wait(float waitDuration)
    {
        this.waitDuration = waitDuration;
    }

    protected override NodeResult Execute()
    {
        if (waitDuration <= 0)
            return NodeResult.Success;

        elapsedTime = 0f;

        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitDuration)
            return NodeResult.Success;

        return NodeResult.InProgress;
    }
}
