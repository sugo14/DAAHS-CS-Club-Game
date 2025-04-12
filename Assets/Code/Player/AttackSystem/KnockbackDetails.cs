/// <summary>
/// All knockback details for a given hitbox.
/// Note that the dealt damage of an attack typically does not significantly affect its knockback.
/// As a result, attacks with high damage that are intended to have a high knockback still must have high knockback values.
/// </summary>
[System.Serializable]
public struct KnockbackDetails
{
    public KnockbackDetails(float baseKnockback, float perPercentIncrease, float angle)
    {
        this.baseKnockback = baseKnockback;
        this.perPercentIncrease = perPercentIncrease;
        this.angle = angle;
    }

    /// <summary>
    /// The knockback amount at 0% damage.
    /// </summary>
    public float baseKnockback;

    /// <summary>
    /// The knockback amount increase per 1% of damage on the target (after this attack's damage is dealt).
    /// </summary>
    public float perPercentIncrease;

    /// <summary>
    /// The angle of the knockback. Consistent with Unity's 2D coordinate system.
    /// 0 degrees is forward, 90 degrees is up, 180 degrees is backward, and 270 degrees (or -90 degrees) is down.
    /// </summary>
    public float angle;
}
