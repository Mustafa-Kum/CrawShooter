using System;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Renderer mesh;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private Color damageEmissionColor;
    [SerializeField] private float blinkSpeed = 2f;
    [SerializeField] private string emissionColorName = "_Addition";
    private Color originalEmissionColor;
    private Material originalMaterial;

    void Start()
    {
        originalMaterial = mesh.material;
        mesh.material = new Material(originalMaterial);

        originalEmissionColor = mesh.material.GetColor(emissionColorName);

        healthComponent.onTakeDamage += TookDamage;
    }

    void Update()
    {
        Color currentEmissionColor = mesh.material.GetColor(emissionColorName);
        Color newEmissionColor = Color.Lerp(currentEmissionColor, originalEmissionColor, Time.deltaTime * blinkSpeed);

        mesh.material.SetColor(emissionColorName, newEmissionColor);
    }

    protected virtual void TookDamage(float health, float delta, float maxHealth, GameObject Instigator)
    {
        Color currentEmissionColor = mesh.material.GetColor(emissionColorName);

        if (Mathf.Abs((currentEmissionColor - originalEmissionColor).grayscale) < 0.1f)
        {
            mesh.material.SetColor(emissionColorName, damageEmissionColor);
        }
    }

}
