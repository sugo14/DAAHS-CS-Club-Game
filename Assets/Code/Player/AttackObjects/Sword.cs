using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    //Owning class, script, and collider
    public ClassBase OwningClassScript;
    private BoxCollider2D projectileCollider;
    private BoxCollider2D ownerCollider;
    //Damage stats for when it hits
    private float damageAmount = 5;
    private float attackStrength = 1;

    public Vector2 targetPosition;
    public float speed = 5f;

    void Update()
    {
        // Move the object a little closer to the target each frame
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        if ((Vector2)transform.localPosition == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetUp(ClassBase OwningClassScriptIn, Vector2 targetPositionIn, float speedIn, float damage, float attackStrengthIn)
    {
        OwningClassScript = OwningClassScriptIn;
        targetPosition = targetPositionIn;
        speed = speedIn;
        damageAmount = damage;
        attackStrength = attackStrengthIn;
    }

    // Called on collison with another object
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Get hit objects attack physics script
        AttackPhysics attackScript = collider.gameObject.GetComponent<AttackPhysics>();
        if (attackScript != null)
        {
            // Hit the object it it colliding with doing damage and knockback
            attackScript.OnHit(damageAmount, attackStrength, transform.position);
            // Update total damge stat on player that made projectile
            OwningClassScript.AddTotalDamage(damageAmount * attackStrength);
            // Destory the Projectile
            Destroy(gameObject);
        }
    }
}
