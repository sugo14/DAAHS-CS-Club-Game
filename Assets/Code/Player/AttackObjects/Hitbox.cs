using System.Collections;
using UnityEngine;

[System.Serializable]
public class HitboxProfile : MonoBehaviour
{
    public Collider2D hitbox;
    public float DamageAmount = 10;
    public float AttackStrength = 1;
    public int StartFrame = 0;
    public int Duration = 5;
    public bool DestroyOnHit = true;

    Attack owningAttack;

    public void Initialize(Attack attack)
    {
        owningAttack = attack;
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
            if (DestroyOnHit) { Destroy(gameObject); }
        }
    }

    IEnumerator OnAttackBegin()
    {
        hitbox.enabled = false;
        for (int i = 0; i < StartFrame; i++) { yield return null; }
        hitbox.enabled = true;
        for (int i = 0; i < Duration; i++) { yield return null; }
        hitbox.enabled = false;
    }
}
