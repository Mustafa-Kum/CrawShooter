using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [Header("Components")]
    [Space]
    [SerializeField] private GameObject[] objectToSpawn;
    [SerializeField] private Transform spawnTransform;

    [Header("Audio")]
    [Space]
    [SerializeField] private AudioClip spawnAudio;
    [SerializeField] private float spawnAudioVolume;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool StartSpawn()
    {
        if (objectToSpawn.Length == 0)
            return false;

        if (animator != null)
            animator.SetTrigger("Spawn");
        else
            Spawn();

        Vector3 spawnAudioLocation = transform.position;
        GamePlay.PlayAudioAtLocation(spawnAudio, spawnAudioLocation, spawnAudioVolume);

        return true;
    }

    public void Spawn()
    {
        if (objectToSpawn.Length == 0)
            return;

        int randomIndex = Random.Range(0, objectToSpawn.Length);

        GameObject newSpawn = Instantiate(objectToSpawn[randomIndex], spawnTransform.position, spawnTransform.rotation);

        ISpawnInterface newSpawnInterface = newSpawn.GetComponent<ISpawnInterface>();

        if (newSpawnInterface != null)
            newSpawnInterface.SpawnedBy(gameObject);
    }
}
