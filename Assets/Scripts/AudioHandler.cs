using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    public void playAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
