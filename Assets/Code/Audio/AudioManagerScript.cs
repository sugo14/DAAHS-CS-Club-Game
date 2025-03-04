using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Audio system by Brackeys

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

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public static void PlaySound(string name)
    {
        Sound sound = Array.Find(instance.sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Attempted to play sound \"{name}\" that does not exist.");
        }
        sound.source.Play();
    }
}
