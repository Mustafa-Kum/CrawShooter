using UnityEngine;

public class Projectile : MonoBehaviour, ITeamInterface
{
    [Header("Components")]
    [Space]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private DamageComponent damageComponent;
    [SerializeField] private ParticleSystem explosionVFX;
    [SerializeField] private float flightHeight;

    private ITeamInterface instigatorTeamInterface;

    public void Launch(GameObject instigator, Vector3 destination)
    {
        instigatorTeamInterface = instigator.GetComponent<ITeamInterface>();

        if (instigatorTeamInterface != null)
            damageComponent.SetTeamInterfaceSrc(instigatorTeamInterface);

        float gravity = Physics.gravity.magnitude;
        float halfFlightTime = Mathf.Sqrt((flightHeight * 2.0f) / gravity);

        Vector3 destinationVector = destination - transform.position;

        destinationVector.y = 0;

        float horizontalDistance = destinationVector.magnitude;
        float upSpeed = halfFlightTime * gravity;
        float forwardSpeed = horizontalDistance / (halfFlightTime * 2.0f);

        Vector3 flightVelocity = Vector3.up * upSpeed + destinationVector.normalized * forwardSpeed;

        rigidBody.AddForce(flightVelocity, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (instigatorTeamInterface.GetRelationTowards(other.gameObject) != ETeamRelation.Friendly)
            Explode();
    }

    private void Explode()
    {
        Vector3 spawnPosition = transform.position;

        Instantiate(explosionVFX, spawnPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
