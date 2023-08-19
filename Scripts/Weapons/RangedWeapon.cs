using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Components")]
    [Space]
    [SerializeField] private AimComponent aimComponent;
    [SerializeField] private ParticleSystem bulletVFX;
    [SerializeField] private ParticleSystem muzzleFlashVFX;
    [SerializeField] private float damage = 5f;

    public override void Attack()
    {
        GameObject target = aimComponent.GetAimTarget(out Vector3 aimDirection);

        if (target != null)
            DamageGameObject(target, damage);

        bulletVFX.transform.rotation = Quaternion.LookRotation(aimDirection);
        bulletVFX.Emit(bulletVFX.emission.GetBurst(0).maxCount);
        muzzleFlashVFX.Emit(muzzleFlashVFX.emission.GetBurst(0).maxCount);

        PlayWeaponAudio();
    }
}
