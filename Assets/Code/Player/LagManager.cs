using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of the various lag states of the player, including whether the player can change momentum, jump, or is held stationary.
/// Lag states are stacked, and when multiple lag states are active, the player will be affected by the most restrictive aspects of all of them.
/// Other scripts are responsible for applying effects based on the information in this class.
/// </summary>
public class LagManager : MonoBehaviour
{
    [SerializeField] ParticleSystem lagParticles;

    public bool DisableMomentumChange { get { return disableMomentumChange == 0; } }
    public bool CanJump { get { return disableJump == 0; } }
    public bool NotForcedStationary { get { return forceStationary == 0; } }

    int disableMomentumChange;
    int disableJump;
    int forceStationary;

    List<LagProfile> lags;

    public void AddLag(LagProfile lag)
    {
        if (lag.disableMomentumChange) { disableMomentumChange++; }
        if (lag.disableJump) { disableJump++; }
        if (lag.forceStationary) { forceStationary++; }
        lags.Add(new LagProfile(lag));
    }

    public void AddLag(LagProfile[] lagProfiles)
    {
        foreach (LagProfile lag in lagProfiles) { AddLag(lag); }
    }

    void Start()
    {
        disableMomentumChange = 0;
        disableJump = 0;
        forceStationary = 0;
        lags = new List<LagProfile>();
    }

    void Update()
    {
        for (int i = 0; i < lags.Count; i++)
        {
            LagProfile lag = lags[i];
            lag.frames--;
            if (lag.frames <= 0)
            {
                if (lag.disableMomentumChange) { disableMomentumChange--; }
                if (lag.disableJump) { disableJump--; }
                if (lag.forceStationary) { forceStationary--; }
                lags.RemoveAt(i);
                i--;
            }
        }

        // Emit lag particles if there are any lags active
        ParticleSystem.EmissionModule emission = lagParticles.emission;
        emission.enabled = lags.Count > 0;
    }
}
