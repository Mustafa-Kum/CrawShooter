using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class Weapon : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private AnimatorOverrideController overrideController;
    public GameObject owner { get; private set; }
    [SerializeField] private string attachSlotTag;

    [Header("Audio")]
    [Space]
    [SerializeField] private AudioClip weaponAudio;
    [SerializeField] private float weaponAudioVolume;

    [Header("AttackRate")]
    [Space]
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float minAttackRate = 1f;
    [SerializeField] private float maxAttackRate = 5f;

    private AudioSource weaponAudioSource;

    private void Awake()
    {
        weaponAudioSource = GetComponent<AudioSource>();
    }

    public void PlayWeaponAudio()
    {
        weaponAudioSource.PlayOneShot(weaponAudio, weaponAudioVolume);
    }

    public abstract void Attack();

    public string GetAttackSlotTag()
    {
        return attachSlotTag;
    }

    public void Init(GameObject owner)
    {
        this.owner = owner;

        Unequip();
    }

    public void Equip()
    {
        gameObject.SetActive(true);

        Animator ownerAnimator = owner.GetComponent<Animator>();
        ownerAnimator.runtimeAnimatorController = overrideController;
        ownerAnimator.SetFloat("attackRate", attackRate);   
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
    }

    public void DamageGameObject(GameObject objectToDamage, float value)
    {
        if (objectToDamage.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        HealthComponent healthComp = objectToDamage.GetComponent<HealthComponent>();

        if (healthComp != null)
            healthComp.ChangeHealth(-value, owner);
    }

    internal void AddRateFire(float boostValue)
    {
        attackRate += boostValue;
        attackRate = Mathf.Clamp(attackRate, minAttackRate, maxAttackRate);

        Animator ownerAnimator = owner.GetComponent<Animator>();
        ownerAnimator.runtimeAnimatorController = overrideController;
        ownerAnimator.SetFloat("attackRate", attackRate);
    }
}
