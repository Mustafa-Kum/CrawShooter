using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/RateFireBoost")]

public class RateFireBoostAbility : Ability
{
    private Weapon weapon;

    [Header("Values")]
    [Space]
    [SerializeField] private float boostValue = 0.2f;
    [SerializeField] private float boostDuration = 5f;

    public override void ActivateAbility()
    {
        if (!TryActivateAbility())
            return;

        InitializeWeapon();
        ApplyRateFireBoost();
        AbilityComp.StartCoroutine(ResetSpeed());
    }

    private void InitializeWeapon()
    {
        weapon = AbilityComp.GetComponent<Player>().inventoryComponent.GetActiveWeapon();
    }

    private void ApplyRateFireBoost()
    {
        weapon.AddRateFire(boostValue);
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(boostDuration);
        weapon.AddRateFire(-boostValue);
    }
}
