/// <summary>
/// A phase of lag resulting from an attack.
/// </summary>
[System.Serializable]
public class LagProfile
{
    public LagProfile(float frames, bool disableMomentumChange, bool disableJump, bool forceStationary)
    {
        this.frames = frames;
        this.disableMomentumChange = disableMomentumChange;
        this.disableJump = disableJump;
        this.forceStationary = forceStationary;
    }

    public LagProfile(LagProfile lagProfile)
    {
        frames = lagProfile.frames;
        disableMomentumChange = lagProfile.disableMomentumChange;
        disableJump = lagProfile.disableJump;
        forceStationary = lagProfile.forceStationary;
    }

    public float frames;
    public bool disableMomentumChange;
    public bool disableJump;
    public bool forceStationary;
}
