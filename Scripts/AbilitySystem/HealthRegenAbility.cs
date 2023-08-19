using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/HealthRegen")]

public class HealthRegenAbility : Ability
{
    [Header("Values")]
    [Space]
    [SerializeField] private float healthRegenValue;
    [SerializeField] private float healthRegenDuration;

    public override void ActivateAbility()
    {
        if (!TryActivateAbility())
            return;

        HealthComponent healthComp = AbilityComp.GetComponent<HealthComponent>();

        if (healthComp != null)
        {
            if (healthRegenDuration == 0)
                healthComp.ChangeHealth(healthRegenValue, AbilityComp.gameObject);
            else
                AbilityComp.StartCoroutine(StartHealthRegen(healthRegenValue, healthRegenDuration, healthComp));
        }
    }

    IEnumerator StartHealthRegen(float value, float duration, HealthComponent healthComponent)
    {
        float counter = duration;
        float regenRate = value / duration;

        while (counter > 0)
        {
            counter -= Time.deltaTime;

            healthComponent.ChangeHealth(regenRate * Time.deltaTime, AbilityComp.gameObject);

            yield return new WaitForEndOfFrame();
        }
    }
}
