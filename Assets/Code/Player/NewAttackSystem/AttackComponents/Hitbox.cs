using System.Collections;
using UnityEngine;

/// <summary>
/// Manages a hitbox of an attack.
/// </summary>
[System.Serializable]
public class HitboxProfile : MonoBehaviour
{
    [SerializeField] Collider2D hitbox;
    [SerializeField] float DamageAmount = 10;
    [SerializeField] float AttackStrength = 1;
    [SerializeField] int StartFrame = 0;
    [SerializeField] int Duration = 5;
    [SerializeField] bool DestroyAttackOnHit = false;
    [SerializeField] bool DestroyHitboxOnHit = true;

    Attack owningAttack;
    SpriteRenderer[] sprites;

    public void Initialize(Attack attack)
    {
        owningAttack = attack;
        if (hitbox == null) { hitbox = GetComponent<Collider2D>(); }
        sprites = GetComponentsInChildren<SpriteRenderer>();

        Physics2D.IgnoreCollision(hitbox, attack.OwningPlayer.GetComponent<Collider2D>());
        StartCoroutine(OnAttackBegin());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        AttackPhysics attackScript = collider.gameObject.GetComponent<AttackPhysics>();
        if (attackScript != null)
        {
            // Hit the object it collides with doing damage and knockback
            attackScript.OnHit(DamageAmount, AttackStrength, transform.position);
            
            // Update total damge stat on player that made projectile
            owningAttack.OwningPlayer.AddTotalDamage(DamageAmount * AttackStrength);

            if (DestroyAttackOnHit) { Destroy(owningAttack.gameObject); }
            if (DestroyHitboxOnHit) { Destroy(gameObject); }
        }
    }

    IEnumerator OnAttackBegin()
    {
        hitbox.enabled = false;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = false; }

        for (int i = 0; i < StartFrame; i++) { yield return null; }
        hitbox.enabled = true;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = true; }

        for (int i = 0; i < Duration; i++) { yield return null; }
        hitbox.enabled = false;
        foreach (SpriteRenderer sprite in sprites) { sprite.enabled = false; }
    }
}
