using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public static class GamePlay
{
    private class AudioSourceContext : MonoBehaviour { }

    private static ObjectPool<AudioSource> audioPool = new ObjectPool<AudioSource>(CreateAudioSource, null, null, DestroyAudioSource, false, 5, 10);

    public static void GameStarted()
    {
        audioPool = new ObjectPool<AudioSource>(CreateAudioSource, null, null, DestroyAudioSource, false, 5, 10);
    }

    public static void DestroyAudioSource(AudioSource audioSource)
    {
        GameObject.Destroy(audioSource.gameObject);
    }

    private static AudioSource CreateAudioSource()
    {
        GameObject audioSourceGameObject = new GameObject("AudioSourceGameObject", typeof(AudioSource), typeof(AudioSourceContext));

        AudioSource audioSource = audioSourceGameObject.GetComponent<AudioSource>();
        audioSource.volume = 1.0f;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        return audioSource;
    }

    public static void SetGamePaused(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;
    }

    public static void PlayAudioAtLocation(AudioClip audioToPlay, Vector3 playLocation, float volume)
    {
        AudioSource newAudioSource = audioPool.Get();
        newAudioSource.volume = volume;
        newAudioSource.gameObject.transform.position = playLocation;
        newAudioSource.PlayOneShot(audioToPlay);

        newAudioSource.GetComponent<AudioSourceContext>().StartCoroutine(ReleaseAudioSource(newAudioSource, audioToPlay.length));
    }

    private static IEnumerator ReleaseAudioSource(AudioSource newAudioSource, float length)
    {
        yield return new WaitForSeconds(length);

        audioPool.Release(newAudioSource);
    }

    internal static void PlayAudioAtPlayer(AudioClip abilityAudio, float abilityAudioVolume)
    {
        PlayAudioAtLocation(abilityAudio, Camera.main.transform.position, abilityAudioVolume);
    }
}
