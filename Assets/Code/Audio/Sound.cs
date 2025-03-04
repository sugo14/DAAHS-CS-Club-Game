using UnityEngine;
using UnityEngine.Audio;

// Audio system by Brackeys

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public bool loop = false;
    public float volume = 1, pitch = 1;

    [HideInInspector]
    public AudioSource source;
}
