using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's response to being hit by an attack.
/// </summary>
public class AttackPhysics : MonoBehaviour
{
    public PlayerSplash playerSplash;
    public LagManager lagManager;

    // The equivlent to your % in Smash Bros
    public float Damage;
    public bool IsInvulnerable;

    Rigidbody2D RB;
    List<HitData> hitDataThisFrame;

    /// <summary>
    /// Adds an attack to the player for processing, dealing damage and applying knockback if successful.
    /// This function is called by attack components when the player is hit.
    /// The HitData with the highest priority this frame will be used.
    /// </summary>
    /// <param name="hitData">The hit data to add for processing.</param>
    public void OnHit(HitData hitData) { hitDataThisFrame.Add(hitData); }

    void Start()
    {
        Damage = 0;
        // Initialize components
        RB = GetComponent<Rigidbody2D>();
        hitDataThisFrame = new List<HitData>();
    }

    void Update()
    {
        ProcessHitDataThisFrame();
    }

    /// <summary>
    /// Processes the hit data for this frame, applying the only the single highest priority hit data to the player.
    /// In the case of ties, the earlier hit data will be used.
    /// </summary>
    void ProcessHitDataThisFrame()
    {
        if (hitDataThisFrame.Count == 0) { return; }

        string logString = Time.time + " Priorities this frame: " + hitDataThisFrame[0].priority.ToString() + " ";
        
        HitData highestPriorityHitData = new HitData(hitDataThisFrame[0]);
        for (int i = 1; i < hitDataThisFrame.Count; i++)
        {
            logString += hitDataThisFrame[i].priority.ToString() + " ";
            if (hitDataThisFrame[i].priority > highestPriorityHitData.priority)
            {
                highestPriorityHitData = new HitData(hitDataThisFrame[i]);
            }
        }

        logString += "  Highest priority: " + highestPriorityHitData.priority.ToString();
        Debug.Log(logString);

        ApplyHitData(highestPriorityHitData);
        hitDataThisFrame.Clear();
    }

    void ApplyHitData(HitData hitData)
    {
        if (RB != null && IsInvulnerable == false)
        {
            AudioManager.PlaySound("Hit1");

            Damage += hitData.damageAmount;

            NewKnockbackFormula(hitData.knockbackDetails);

            // Apply lag to the player
            lagManager.AddLag(hitData.lagProfiles);

            if (playerSplash != null)  { playerSplash.SetPercent((int)Damage); }  
        }
    }

    void NewKnockbackFormula(KnockbackDetails knockbackDetails)
    {
        // Pseudo-smash bros knockback formula
        float force = knockbackDetails.baseKnockback + (Damage * knockbackDetails.perPercentIncrease);
        float x = force * Mathf.Cos(knockbackDetails.angle * Mathf.Deg2Rad);
        float y = force * Mathf.Sin(knockbackDetails.angle * Mathf.Deg2Rad);

        // We do not apply a force here because stacked knockback is very sketchy
        Vector2 newVelocity = new Vector2(x, y);
        RB.velocity = newVelocity;
    }
}
