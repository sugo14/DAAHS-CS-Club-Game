using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages hitbox collisions and damage dealing of an attack.
/// </summary>
[System.Serializable]
public class HitboxProfile : MonoBehaviour
{
    public float damageAmount = 10;
    public float attackStrength = 1;
    public bool initializeOnAttackBegin = true;
    public KnockbackDetails knockbackDetails = new KnockbackDetails();

    // I don't know why this was added
    /* public Collider2D LastCollision { get; private set; } */

    [SerializeField] Collider2D hitbox;
    [SerializeField] int startFrame = 0;
    [SerializeField] int duration = 5;
    [SerializeField] int hitboxPriority = 0;
    [SerializeField] UnityEvent<Collider2D> onHitEvents;

    Attack owningAttack;
    SpriteRenderer[] sprites;

    public void Initialize(Attack attack)
    {
        owningAttack = attack;
        if (hitbox == null) { hitbox = GetComponent<Collider2D>(); }
        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (attack.FacingDirection == Facing.Left)
        {
            // Flip the knockback angle horizontally
            knockbackDetails.angle = 180 - knockbackDetails.angle;
        }

        Physics2D.IgnoreCollision(hitbox, attack.OwningPlayer.GetComponent<Collider2D>());
        StartCoroutine(OnAttackBegin());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        onHitEvents.Invoke(collider);

        AttackPhysics attackScript = collider.gameObject.GetComponent<AttackPhysics>();
        if (attackScript != null)
        {
            // Hit the object it collides with doing damage and knockback
            attackScript.OnHit(damageAmount, knockbackDetails);
            
            // Update total damge stat on player that made attack
            /* owningAttack.OwningPlayer.AddTotalDamage(damageAmount * attackStrength); */
        }
    }

    IEnumerator OnAttackBegin()
    {
        hitbox.enabled = false;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = false; }

        for (int i = 0; i < startFrame; i++) { yield return null; }
        hitbox.enabled = true;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = true; }

        for (int i = 0; i < duration; i++) { yield return null; }
        hitbox.enabled = false;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = false; }
    }
}
