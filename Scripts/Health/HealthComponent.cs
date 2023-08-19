using UnityEngine;

public class HealthComponent : MonoBehaviour, IRewardListener
{
    [Header("Health Values")]
    [Space]
    [SerializeField] private float currentHealth = 100;
    [SerializeField] private float maxHealth = 100;

    [Header("Audio")]
    [Space]
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private float audioVolume;

    public delegate void OnHealthChange(float health, float delta, float maxHealth);
    public delegate void OnTakeDamage(float health, float delta, float maxHealth, GameObject Instigator);
    public delegate void OnDead(GameObject killer);

    public event OnHealthChange onHealthChange;
    public event OnTakeDamage onTakeDamage;
    public event OnDead onDead;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HealthValueUpdate()
    {
        onHealthChange?.Invoke(currentHealth, 0, maxHealth);
    }

    public void ChangeHealth(float value, GameObject Instigator)
    {
        if (value == 0f || currentHealth == 0f)
            return;

        currentHealth += value;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (value < 0)
        {
            onTakeDamage?.Invoke(currentHealth, value, maxHealth, Instigator);

            Vector3 location = transform.position;

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(hitAudio, audioVolume);
            
            //GamePlay.PlayAudioAtLocation(hitAudio, location, audioVolume);
        }

        onHealthChange?.Invoke(currentHealth, value, maxHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            onDead?.Invoke(Instigator);

            Vector3 location = transform.position;

            GamePlay.PlayAudioAtLocation(deathAudio, location, audioVolume);
        }
    }

    public void Reward(Reward reward)
    {
        //health = Mathf.Clamp(health + reward.healthReward, 0, maxHealth); 
        //onHealthChange?.Invoke(health, reward.healthReward, maxHealth);
    }
}
