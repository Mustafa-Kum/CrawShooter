using UnityEngine;

public interface IBehaviorTreeInterface
{
    public void RotateTowards(GameObject target, bool verticalAim = false);

    public void AttackTarget(GameObject target);
}
