using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName = "Sounds/SoundEffect")]
public class SoundEffect : ScriptableObject
{
    public AudioClip clip;
    public float startTime;
    public float speed = 1;
    public float volume = 1;
}
