using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour, IPurchaseListener, IRewardListener
{
    [Header("Abilties")]
    [Space]
    [SerializeField] private Ability[] abilities;

    [Header("Stamina Info")]
    [Space]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float maxStamina = 100f;

    private float staminaIncreaseRate = 2.5f;
    private float currentStamina;

    private List<Ability> abilitiesList = new List<Ability>();

    public delegate void OnNewAbilityAdded(Ability newAbility);
    public delegate void OnStaminaChanged(float newValue, float maxValue);

    public event OnNewAbilityAdded onNewAbilityAdded;
    public event OnStaminaChanged onStaminaChanged;


    void Start()
    {
        InitializeAbilities();
        currentStamina = stamina;
    }

    private void InitializeAbilities()
    {
        foreach (Ability ability in abilities)
        {
            GiveAbilities(ability);
        }
    }

    private void GiveAbilities(Ability ability)
    {
        Ability newAbility = Instantiate(ability);
        newAbility.InitAbility(this);
        abilitiesList.Add(newAbility);
        onNewAbilityAdded?.Invoke(newAbility);
    }

    public void ActivateAbility(Ability abilityToActivate)
    {
        if (abilitiesList.Contains(abilityToActivate))
        {
            abilityToActivate.ActivateAbility();
        }
    }

    public bool TryConsumeStamina(float staminaToConsume)
    {
        if (stamina <= staminaToConsume)
            return false;

        stamina -= staminaToConsume;
        BroadcastStaminaChanged();

        return true;
    }

    void Update()
    {
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        float staminaDecrease = staminaIncreaseRate * Time.deltaTime * 1;

        if (stamina <= maxStamina)
            stamina = Mathf.Max(stamina + staminaDecrease, 0f);

        BroadcastStaminaChanged();
    }

    public void BroadcastStaminaChanged()
    {
        onStaminaChanged?.Invoke(stamina, maxStamina);
    }

    public bool HandlePurchase(Object newPurchase)
    {
        Ability itemAsAbility = newPurchase as Ability;

        if (itemAsAbility == null)
            return false;

        GiveAbilities(itemAsAbility);

        return true;
    }

    public void Reward(Reward reward)
    {
        // ---> Stamina Reward onKill
        //stamina = Mathf.Clamp(stamina + reward.staminaReward, 0, maxStamina);
        //BroadcastStaminaChanged();
    }

    private float GetStamina() => stamina;
}
