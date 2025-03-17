using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerStateSound
{
    public PlayerState state;
    public SoundProfile soundProfile;
}

public class PlayerAudioManager : MonoBehaviour
{
    public PlayerStateManager stateScript;
    public List<PlayerStateSound> stateSounds;

    void Awake()
    {
        foreach (PlayerStateSound stateSound in stateSounds)
        {
            stateSound.soundProfile.Initialize();
        }
    }

    void Update()
    {
        PlayerState state = stateScript.playerState;

        foreach (PlayerStateSound stateSound in stateSounds)
        {
            stateSound.soundProfile.Update(stateSound.state == state);
        }
    }
}
