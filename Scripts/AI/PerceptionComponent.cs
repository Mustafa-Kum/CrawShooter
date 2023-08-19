using System.Collections.Generic;
using UnityEngine;

public class PerceptionComponent : MonoBehaviour
{
    [SerializeField] private SenseComponent[] senses;

    private LinkedList<Perception> currentlyPerceivedPerception = new LinkedList<Perception>();
    private Perception targetPerception;

    public delegate void OnPerceptionTargetChanged(GameObject target, bool sensed);
    public event OnPerceptionTargetChanged onPerceptionTargetChanged;

    private void Awake()
    {
        foreach (SenseComponent sense in senses)
        {
            sense.onPerceptionUpdated += SenseUpdated;
        }
    }

    private void SenseUpdated(Perception perception, bool succsessfulySensed)
    {
        var nodeFound = currentlyPerceivedPerception.Find(perception);

        if (succsessfulySensed)
        {
            if (nodeFound != null)
            {
                currentlyPerceivedPerception.AddAfter(nodeFound, perception);
            }
            else
            {
                currentlyPerceivedPerception.AddLast(perception);
            }
        }
        else
        {
            currentlyPerceivedPerception.Remove(nodeFound);
        }

        UpdateTargetPerception();
    }

    private void UpdateTargetPerception()
    {
        if (currentlyPerceivedPerception.Count != 0)
        {
            Perception highestPerception = currentlyPerceivedPerception.First.Value;

            if (targetPerception == null || targetPerception != highestPerception)
            {
                targetPerception = highestPerception;
                onPerceptionTargetChanged?.Invoke(targetPerception.gameObject, true);
            }
        }
        else
        {
            if (targetPerception != null)
            {
                onPerceptionTargetChanged?.Invoke(targetPerception.gameObject, false);
                targetPerception = null;
            }
        }
    }

    internal void AssignPercievedPerception(Perception targetPerception)
    {
        if (senses.Length != 0)
        {
            senses[0].AssignPercievedPerception(targetPerception);
        }
    }
}
