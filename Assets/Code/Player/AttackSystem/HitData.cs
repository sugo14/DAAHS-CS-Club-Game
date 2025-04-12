[System.Serializable]
public class HitData
{
    public int priority = 0;
    public float damageAmount = 10;
    public KnockbackDetails knockbackDetails = new KnockbackDetails(20, 0.3f, 45);
    public LagProfile[] lagProfiles = new LagProfile[1] { new LagProfile(20, true, true, false) };

    public HitData(int priority, float damageAmount, KnockbackDetails knockbackDetails, LagProfile[] lagProfiles)
    {
        this.priority = priority;
        this.damageAmount = damageAmount;
        this.knockbackDetails = knockbackDetails;
        this.lagProfiles = lagProfiles;
    }

    public HitData(HitData hitData)
    {
        priority = hitData.priority;
        damageAmount = hitData.damageAmount;
        knockbackDetails = hitData.knockbackDetails;
        lagProfiles = new LagProfile[hitData.lagProfiles.Length];
        for (int i = 0; i < hitData.lagProfiles.Length; i++)
        {
            lagProfiles[i] = new LagProfile(hitData.lagProfiles[i]);
        }
    }

    public HitData()
    {
        priority = 0;
        damageAmount = 10;
        knockbackDetails = new KnockbackDetails(20, 0.3f, 45);
        lagProfiles = new LagProfile[1] { new LagProfile(20, true, true, false) };
    }
}
