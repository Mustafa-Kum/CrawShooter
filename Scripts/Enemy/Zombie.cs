using UnityEngine;

public class Zombie : Enemy
{
    [SerializeField] private TriggerDamageComponent damageComponent;

    protected override void Start()
    {
        base.Start();
        damageComponent.SetTeamInterfaceSrc(this);
    }

    public override void AttackTarget(GameObject target)
    {
        Animator.SetTrigger("Attack");
    }

    public void AttackPoint()
    {
        if (damageComponent)
            damageComponent.SetDamageEnabled(true);
    }

    public void AttackEnd()
    {
        if (damageComponent)
            damageComponent.SetDamageEnabled(false);
    }
}
