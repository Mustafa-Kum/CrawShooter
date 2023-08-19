using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    private Ability ability;

    [Header("Components")]
    [Space]
    [SerializeField] private RectTransform offsetPoint;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private Image abilityCooldown;

    [Header("IconHighlight")]
    [SerializeField] private float highlightSize = 1.5f;
    [SerializeField] private float highlightOffSet = 200f;
    [SerializeField] private float scaleSpeed = 20f;

    private Vector3 goalScale = Vector3.one;
    private Vector3 goalOffSet = Vector3.zero;
    private bool skillIsOnCooldown = false;
    private float cooldownCounter = 0;

    void Update()
    {
        UpdateUIEffects();
    }

    private void UpdateUIEffects()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, goalScale, Time.deltaTime * scaleSpeed);
        offsetPoint.localPosition = Vector3.Lerp(offsetPoint.localPosition, goalOffSet, Time.deltaTime * scaleSpeed);
    }

    internal void Init(Ability newAbility)
    {
        ability = newAbility;
        abilityIcon.sprite = newAbility.GetAbilityIcon();

        abilityCooldown.enabled = false;

        ability.onCooldownStarted += StartCooldown;
    }

    private void StartCooldown()
    {
        if (skillIsOnCooldown)
            return;

        StartCoroutine(CooldownCoroutine());
    }

    internal void ActivateAbility()
    {
        ability.ActivateAbility();
    }

    IEnumerator CooldownCoroutine()
    {
        skillIsOnCooldown = true;
        cooldownCounter = ability.GetCooldownDuration();
        float cooldownDuration = cooldownCounter;

        abilityCooldown.enabled = true;

        while (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            abilityCooldown.fillAmount = cooldownCounter / cooldownDuration;

            yield return new WaitForEndOfFrame();
        }

        skillIsOnCooldown = false;
        abilityCooldown.enabled = false;
    }

    public void SetScaleValue(float scaleValue)
    {
        goalScale = Vector3.one * (1 + (highlightSize - 1) * scaleValue);
        goalOffSet = Vector3.left * highlightOffSet * scaleValue;
    }
}
