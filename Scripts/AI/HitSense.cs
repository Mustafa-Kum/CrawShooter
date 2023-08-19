using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSense : SenseComponent
{
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private float hitMemory = 3f;

    private Dictionary<Perception, Coroutine> hitRecord = new Dictionary<Perception, Coroutine>();

    protected override bool IsPerceptionSensable(Perception perception)
    {
        return hitRecord.ContainsKey(perception);
    }

    void Start()
    {
        healthComponent.onTakeDamage += TookDamage;
    }

    private void TookDamage(float health, float delta, float maxHealth, GameObject instigator)
    {
        Perception perception = instigator.GetComponent<Perception>();

        if (perception != null)
        {
            Coroutine newForgettingCoroutine = StartCoroutine(ForgetPerception(perception));

            if (hitRecord.TryGetValue(perception, out Coroutine onGoingCoroutine))
            {
                StopCoroutine(onGoingCoroutine);
                hitRecord[perception] = newForgettingCoroutine;
            }
            else
            {
                hitRecord.Add(perception, newForgettingCoroutine);
            }
        }
    }

    IEnumerator ForgetPerception(Perception perception)
    {
        yield return new WaitForSeconds(hitMemory);
        hitRecord.Remove(perception);
    }
}
