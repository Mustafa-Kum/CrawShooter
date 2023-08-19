using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SpeedBoost")]

public class SpeedBoostAbility : Ability
{
    private Player player;

    [Header("Values")]
    [Space]
    [SerializeField] private float boostValue = 0.2f;
    [SerializeField] private float boostDuration = 5f;

    public override void ActivateAbility()
    {
        if (!TryActivateAbility())
            return;

        InitializePlayer();
        ApplySpeedBoost();
        AbilityComp.StartCoroutine(ResetSpeed());
    }

    private void InitializePlayer()
    {
        player = AbilityComp.GetComponent<Player>();
    }

    private void ApplySpeedBoost()
    {
        player.AddMoveSpeed(boostValue);
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(boostDuration);
        player.AddMoveSpeed(-boostValue);
    }
}
