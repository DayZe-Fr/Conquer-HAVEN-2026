using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    public void playAudio(SoundEffect sound)
    {
        source.clip = sound.clip;
        source.time = Mathf.Clamp(sound.startTime, 0, sound.clip.length);
        source.pitch = sound.speed;
        source.volume = Mathf.Clamp01(sound.volume);

        source.Play();
    }
}
