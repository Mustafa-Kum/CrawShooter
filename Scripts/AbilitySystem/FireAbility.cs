using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Fire")]

public class FireAbility : Ability
{
    [Header("Components")]
    [Space]
    [SerializeField] private Scanner scannerPrefab;
    [SerializeField] private GameObject scanVFX;
    [SerializeField] private GameObject damageVFX;

    [Header("Values")]
    [Space]
    [SerializeField] private float fireRadius;
    [SerializeField] private float fireDuration;
    [SerializeField] private float fireDamage;
    [SerializeField] private float fireDamageDuration;
    
    public override void ActivateAbility()
    {
        if (!TryActivateAbility())
            return;
        
        InitializeFireScanner();
    }

    private void InitializeFireScanner()
    {
        Scanner fireScanner = Instantiate(scannerPrefab, AbilityComp.transform);
        fireScanner.SetScanRange(fireRadius);
        fireScanner.SetScanDuration(fireDuration);
        fireScanner.AddChildAttached(Instantiate(scanVFX).transform);
        fireScanner.onScanDetectionUpdated += DetectionUpdated;
        fireScanner.StartScan();
    }

    private void DetectionUpdated(GameObject newDetection)
    {
        ITeamInterface detectedTeamInterface = newDetection.GetComponent<ITeamInterface>();

        if (detectedTeamInterface == null || detectedTeamInterface.GetRelationTowards(AbilityComp.gameObject) != ETeamRelation.Enemy)
            return;

        HealthComponent enemyHealthComp = newDetection.GetComponent<HealthComponent>();

        if (enemyHealthComp == null)
            return;

        AbilityComp.StartCoroutine(ApplyDamageTo(enemyHealthComp));
    }

    IEnumerator ApplyDamageTo(HealthComponent enemyHealthComp)
    {
        GameObject damageVFXInstance = Instantiate(damageVFX, enemyHealthComp.transform);

        float damageRate = fireDamage / fireDamageDuration;
        float startTime = 0;

        while (startTime < fireDamageDuration && enemyHealthComp != null)
        {
            startTime += Time.deltaTime;

            enemyHealthComp.ChangeHealth(-damageRate * Time.deltaTime, AbilityComp.gameObject);

            yield return new WaitForEndOfFrame();
        }

        if (damageVFXInstance != null)
            Destroy(damageVFXInstance);
    }
}
