using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateSound
{
    public PlayerState state;
    public SoundProfile soundProfile;
}

public class PlayerAudioManager : MonoBehaviour
{
    public PlayerStateScript stateScript;
    public List<PlayerStateSound> stateSounds;

    void Update()
    {
        PlayerState state = stateScript.playerState;

        foreach (PlayerStateSound stateSound in stateSounds)
        {
            stateSound.soundProfile.Update(stateSound.state == state);
        }
    }
}
