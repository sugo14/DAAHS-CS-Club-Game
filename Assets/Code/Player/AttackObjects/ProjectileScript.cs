using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    // Set to true after all other vars are set to make sure it can collide before it fully set up
    public bool IsMoving;
    // Movement amount negative to go opposite dir
    public float HorizontalMovementAmount = 1;
    public float VerticalMovementAmount = 0;
    // Owning class, script, and collider
    public ClassBaseScript OwningClassScript;
    private BoxCollider2D projectileCollider;
    private BoxCollider2D ownerCollider;
    // Damage stats for when it hits
    private float DamageAmount = 5;
    private float AttackStrength = 1;

    public float MaxDistance = 10;
    private float startPositionX;
    private float startPositionY;

    
    // Start is called before the first frame update
    void Start()
    {
        // Get players collider and disable its collsion
        ownerCollider = OwningClassScript.GetComponent<BoxCollider2D>();
        if (OwningClassScript != null)
        {
            projectileCollider = GetComponent<BoxCollider2D>();
        }
        if (projectileCollider != null && ownerCollider != null)
        {
            Physics2D.IgnoreCollision(projectileCollider, ownerCollider);
        }
        else
        {
            UnityEngine.Debug.LogError("Projecile Collider or owner Collider is NUll");
        }

        startPositionX = this.transform.position.x;
        startPositionY = this.transform.position.y;
    }

    void Update()
    {
        if (IsMoving == true)
        {
            MoveProjectile();
        }
    }

    public void SetUp(ClassBaseScript InOwningClassScript, float InDamageAmount, float InAttackStrength, float InMaxDistance, bool LeftRight = false, float InHorizontalMovementAmount = 0, bool UpDown = true, float InVerticalMovementAmount = 0)
    {
        if (LeftRight)
        {
            HorizontalMovementAmount = -InHorizontalMovementAmount;
        }
        else
        {
            HorizontalMovementAmount = InHorizontalMovementAmount;
        }
        if (UpDown)
        {
            VerticalMovementAmount = InVerticalMovementAmount;
        }
        else
        {
            VerticalMovementAmount = -InVerticalMovementAmount;
        }
        OwningClassScript = InOwningClassScript;
        DamageAmount = InDamageAmount;
        AttackStrength = InAttackStrength;
        MaxDistance = InMaxDistance;

        IsMoving = true;
    }

    // Update projectiles position
    void MoveProjectile()
    {
        // Create vector of how to move
        Vector3 MoveOffset = new Vector3(HorizontalMovementAmount, VerticalMovementAmount, 0) * Time.deltaTime;
        
        // update the position
        transform.position = transform.position + MoveOffset;
        //Destory the projectile if the location diffrence is more than the max distance it can travel. 
        if (Mathf.Abs(transform.position.x - startPositionX) >= MaxDistance || Mathf.Abs(transform.position.y - startPositionY) >= MaxDistance)
        {
            Destroy(gameObject);
        }

    }
    // Called on collison with another object
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Get hit objects attack physics script
        AttackPhysicsScript attackScript = collider.gameObject.GetComponent<AttackPhysicsScript>();
        if (attackScript != null)
        {
            // Hit the object it it colliding with doing damage and knockback
            attackScript.OnHit(DamageAmount, AttackStrength, this.transform.position);
            // Update total damge stat on player that made projectile
            OwningClassScript.AddTotalDamage(DamageAmount * AttackStrength);
            // Destory the Projectile
            Destroy(this.gameObject);
        }
    }
}
