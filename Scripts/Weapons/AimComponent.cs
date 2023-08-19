using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private Transform muzzle;
    [SerializeField] private float aimRange = 1000;
    [SerializeField] private LayerMask aimMask;

    public GameObject GetAimTarget(out Vector3 aimDirection)
    {
        Vector3 aimStart = muzzle.position;

        aimDirection = GetAimDirection();

        if (Physics.Raycast(aimStart, aimDirection, out RaycastHit hitInfo, aimRange, aimMask))
            return hitInfo.collider.gameObject;

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(muzzle.position, muzzle.position + GetAimDirection() * aimRange);
    }

    private Vector3 GetAimDirection()
    {
        Vector3 muzzleDir = muzzle.forward;

        return new Vector3(muzzleDir.x, 0f, muzzleDir.z).normalized;
    }
}
