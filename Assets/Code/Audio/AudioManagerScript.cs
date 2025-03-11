using System;
using System.Collections;
using UnityEngine;

// Audio system partially by Brackeys

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public static void PlaySound(string name, float volumeMod = 1f, float pitchMod = 1f)
    {
        Sound sound = Array.Find(instance.sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Attempted to play sound \"{name}\" that does not exist.");
        }

        // this gives us full control over all attributes of the sound
        AudioSource source = instance.gameObject.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.loop = sound.loop;
        source.volume = sound.volume * volumeMod;
        source.pitch = sound.pitch * pitchMod;

        source.Play();
        instance.StartCoroutine(DestroyAfterPlay(source));
    }

    private static IEnumerator DestroyAfterPlay(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }
}
