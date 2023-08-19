using System;
using UnityEngine;

[Serializable]
public class VFXSpec
{
    public ParticleSystem particleSystem;
    public float size;
}

public class Spawner : Enemy
{
    [SerializeField] private VFXSpec[] deathVFX;

    protected override void Dead()
    {
        foreach (VFXSpec spec in deathVFX)
        {
            ParticleSystem particleSystem = Instantiate(spec.particleSystem);
            particleSystem.transform.position = transform.position;
            particleSystem.transform.localScale = Vector3.one * spec.size;
        }
    }
}
