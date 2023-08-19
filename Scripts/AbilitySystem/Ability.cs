using System.Collections;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    private AbilityComponent abilityComponent;

    [Header("Components")]
    [Space]
    [SerializeField] private Sprite abilityIcon;

    [Header("Stamina Info")]
    [Space]
    [SerializeField] private float staminaCost;
    [SerializeField] private float cooldown;

    [Header("Audio")]
    [Space]
    [SerializeField] private AudioClip abilityAudio;
    [SerializeField] private float abilityAudioVolume;

    private bool abilityOnCooldown = false;

    public delegate void OnCooldownStarted();

    public OnCooldownStarted onCooldownStarted;

    public abstract void ActivateAbility();

    public AbilityComponent AbilityComp { get => abilityComponent; private set => abilityComponent = value; }

    internal void InitAbility(AbilityComponent abilityComponent)
    {
        this.abilityComponent = abilityComponent;
    }

    protected bool TryActivateAbility()
    {
        if (abilityOnCooldown || abilityComponent == null || !abilityComponent.TryConsumeStamina(staminaCost))
            return false;

        StartAbilityCooldown();
        PlayAbilityAudio();
        return true;
    }

    private void StartAbilityCooldown()
    {
        abilityComponent.StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        abilityOnCooldown = true;
        onCooldownStarted?.Invoke();
        yield return new WaitForSeconds(cooldown);
        abilityOnCooldown = false;
    }

    private void PlayAbilityAudio()
    {
        GamePlay.PlayAudioAtPlayer(abilityAudio, abilityAudioVolume);
    }

    internal Sprite GetAbilityIcon() => abilityIcon;

    internal float GetCooldownDuration() => cooldown;
}
