using UnityEngine;

[System.Serializable]
public class SoundProfile
{
    public string soundName;
    public bool repeats;
    public float repeatTime;
    bool wasLastActive;
    float timer;

    void Reset()
    {
        timer = 0;
        wasLastActive = false;
    }

    void Initialize()
    {
        Reset();
    }

    void Play()
    {
        Object.FindAnyObjectByType<AudioManager>().PlaySound(soundName);
    }

    public void Update(bool isActive)
    {
        if (!isActive)
        {
            timer = 0;
            wasLastActive = false;
            return;
        }
        if (repeats)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = repeatTime;
                Play();
            }
            return;
        }
        else
        {
            if (!wasLastActive) { Play(); }
            wasLastActive = true;
        }
    }
}
