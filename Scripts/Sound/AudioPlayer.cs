using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioPlayer")]

public class AudioPlayer : ScriptableObject
{
    [Header("Components")]
    [Space]
    [SerializeField] private AudioClip clickAudio;
    [SerializeField] private AudioClip buyAudio;
    [SerializeField] private AudioClip selectAudio;

    public void PlayClick()
    {
        PlayAudio(clickAudio);
    }

    public void PlayBuy()
    {
        PlayAudio(buyAudio);
    }

    public void PlaySelect()
    {
        PlayAudio(selectAudio);
    }

    private void PlayAudio(AudioClip audioToPlay)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioToPlay);
    }
}
