using UnityEngine;

/// <summary>
/// Manages the player's response to being hit by an attack.
/// </summary>
public class AttackPhysics : MonoBehaviour
{
    public PlayerSplash playerSplash;

    // The equivlent to your % in Smash Bros
    public float Damage;

    public bool IsInvulnerable;

    private Rigidbody2D RB;
    private ClassBase ClassScript;

    void Start()
    {
        Damage = 0;
        // Initialize components
        RB = GetComponent<Rigidbody2D>();
        ClassScript = GetComponent<ClassBase>();
    }

    /// <summary>
    /// Performs an attack on the player, dealing damage and applying knockback.
    /// This function is called by attack components when the player is hit.
    /// </summary>
    /// <param name="hitDamage">The amount of damage to be dealt to the player.</param>
    /// <param name="knockbackDetails">The knockback to be performed.</param>
    public void OnHit(float hitDamage, KnockbackDetails knockbackDetails)
    {
        if (RB != null && IsInvulnerable == false)
        {
            AudioManager.PlaySound("Hit1");

            // Deal damage
            Damage += hitDamage;

            NewKnockbackFormula(knockbackDetails);

            if (ClassScript != null) { ClassScript.ResetCharge(); }
            if (playerSplash != null)  { playerSplash.SetPercent((int)Damage); }  
        }
    }

    void NewKnockbackFormula(KnockbackDetails knockbackDetails)
    {
        Debug.Log("Knockback Angle: " + knockbackDetails.angle);
        // Pseudo-smash bros knockback formula
        float force = knockbackDetails.baseKnockback + (Damage * knockbackDetails.perPercentIncrease);
        float x = force * Mathf.Cos(knockbackDetails.angle * Mathf.Deg2Rad);
        float y = force * Mathf.Sin(knockbackDetails.angle * Mathf.Deg2Rad);

        // Apply force
        Vector2 knockbackForce = new Vector2(x, y);
        RB.AddForce(knockbackForce, ForceMode2D.Impulse);
    }
}
