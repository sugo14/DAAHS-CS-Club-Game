using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages hitbox collisions and damage dealing of an attack.
/// </summary>
[System.Serializable]
public class HitboxProfile : AttackComponent
{
    [SerializeField] bool hitMultipleTimes = false;
    [SerializeField] HitData hitData;
    [SerializeField] Collider2D hitbox;
    [SerializeField] int startFrame = 5;
    [SerializeField] int duration = 5;
    [SerializeField] UnityEvent<Collider2D> onHitEvents;

    SpriteRenderer[] sprites;

    public override void Initialize(Attack attack)
    {
        base.Initialize(attack);

        owningAttack = attack;
        if (hitbox == null)
        {
            hitbox = GetComponent<Collider2D>();
            if (hitbox == null)
            {
                Debug.LogError("Warning: hitbox not found on attack.");
            }
        }

        sprites = GetComponentsInChildren<SpriteRenderer>();

        if (attack.FacingDirection == Facing.Left)
        {
            // Flip the knockback angle horizontally
            hitData.knockbackDetails.angle = 180 - hitData.knockbackDetails.angle;
        }

        Physics2D.IgnoreCollision(hitbox, attack.OwningPlayer.GetComponent<Collider2D>());
        StartCoroutine(OnAttackBegin());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        onHitEvents.Invoke(collider);

        AttackPhysics attackScript = collider.gameObject.GetComponent<AttackPhysics>();
        if (attackScript != null && (hitMultipleTimes || !owningAttack.HasHitObject(collider.gameObject)))
        {
            // Hit the object it collides with doing damage and knockback
            attackScript.OnHit(hitData);
            owningAttack.AddHitObject(collider.gameObject);
            
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
