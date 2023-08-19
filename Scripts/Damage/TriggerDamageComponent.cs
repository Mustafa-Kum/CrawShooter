using UnityEngine;

public class TriggerDamageComponent : DamageComponent
{
    [Header("Components")]
    [Space]
    [SerializeField] private BoxCollider trigger;

    [Header("Damage Info")]
    [SerializeField] private float damage;
    [SerializeField] private bool startedEnable = false;

    void Start()
    {
        SetDamageEnabled(startedEnable);
    }

    public void SetDamageEnabled(bool enabled)
    {
        trigger.enabled = enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ShouldDamage(other.gameObject))
            return;

        HealthComponent healthComponent = other.GetComponent<HealthComponent>();

        if (healthComponent != null)
            healthComponent.ChangeHealth(-damage, gameObject);

    }
}
