using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseComponent : MonoBehaviour
{
    static private List<Perception> registredPerception = new List<Perception>();
    private List<Perception> perceivablePerception = new List<Perception>();
    private Dictionary<Perception, Coroutine> forgettingRoutine = new Dictionary<Perception, Coroutine>();

    [SerializeField] private float forgettingTime = 4f;

    public delegate void OnPerceptionUpdated(Perception perception, bool succsessfulySensed);
    public event OnPerceptionUpdated onPerceptionUpdated;

    static public void RegisterPerception(Perception perception)
    {
        if (registredPerception.Contains(perception))
            return;

        registredPerception.Add(perception);
    }

    static public void UnRegisterPerception(Perception perception)
    {
        registredPerception.Remove(perception);
    }

    protected abstract bool IsPerceptionSensable(Perception perception);

    void Update()
    {
        foreach (var perception in registredPerception)
        {
            if (IsPerceptionSensable(perception))
            {
                if (!perceivablePerception.Contains(perception))
                {
                    perceivablePerception.Add(perception);

                    if (forgettingRoutine.TryGetValue(perception, out Coroutine routine))
                    {
                        StopCoroutine(routine);
                        forgettingRoutine.Remove(perception);
                    }
                    else
                    {
                        onPerceptionUpdated?.Invoke(perception, true);
                    }
                }
            }
            else
            {
                if (perceivablePerception.Contains(perception))
                {
                    perceivablePerception.Remove(perception);

                    forgettingRoutine.Add(perception, StartCoroutine(forgetPerception(perception)));
                }
            }
        }
    }

    IEnumerator forgetPerception(Perception perception)
    {
        yield return new WaitForSeconds(forgettingTime);

        forgettingRoutine.Remove(perception);

        onPerceptionUpdated?.Invoke(perception, false);
    }

    protected virtual void DrawDebug()
    {

    }

    private void OnDrawGizmos()
    {
        DrawDebug();
    }

    internal void AssignPercievedPerception(Perception targetPerception)
    {
        perceivablePerception.Add(targetPerception);

        onPerceptionUpdated?.Invoke(targetPerception, true);

        // Forget için buraya bak. 79 8.38

        if (forgettingRoutine.TryGetValue(targetPerception, out Coroutine forgetCoroutine))
        {
            StopCoroutine(forgetCoroutine);

            forgettingRoutine.Remove(targetPerception);
        }
    }
}
