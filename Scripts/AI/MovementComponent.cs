using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 6f;

    public float RotatingAim(Vector3 aimDirection)
    {
        float currentTurnSpeed = 0;

        if (aimDirection.magnitude != 0)
        {
            Quaternion previousRotation = transform.rotation;

            float turnLerpAlpha = turnSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnLerpAlpha);

            Quaternion currentRotation = transform.rotation;
            float dotProduct = Vector3.Dot(aimDirection, transform.right);
            float direction = dotProduct > 0 ? 1 : -1;
            float rotationDelta = Quaternion.Angle(previousRotation, currentRotation) * direction;
            currentTurnSpeed = rotationDelta / Time.deltaTime;
        }

        return currentTurnSpeed;
    }
}
