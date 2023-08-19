using UnityEngine;

public class HealthUIComponent : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private HealthBar healthBarToSpawn;
    [SerializeField] private Transform healthBarAttachPoint;
    [SerializeField] private HealthComponent healthComponent;

    private void Start()
    {
        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        InGameUI inGameUI = FindObjectOfType<InGameUI>();
        HealthBar newHealthBar = Instantiate(healthBarToSpawn, inGameUI.transform);

        newHealthBar.Init(healthBarAttachPoint);

        healthComponent.onHealthChange += newHealthBar.SetHealthSliderValue;
        healthComponent.onDead += newHealthBar.onOwnerDead;
    }
}
