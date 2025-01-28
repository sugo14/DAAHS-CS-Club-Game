using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //Set to true after all other vars are set to make sure it can collide before it fully set up
    public bool IsMoving;
    //Movment amount negative to go opposite dir
    public float HorizontalMovmentAmount = 1;
    public float VerticalMovmentAmount = 0;
    //Owning class, script, and collider
    public ClassBaseScript OwningClassScript;
    private BoxCollider2D projectileCollider;
    private BoxCollider2D ownerCollider;
    //Damage stats for when it hits
    public float DamageAmount = 5;
    public float AttackStrength = 1;

    // Start is called before the first frame update
    void Start()
    {
        //Get players collider and disable its collsion
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
    }

    // Update is called once per frame
    void Update()
    {

        if (IsMoving == true)
        {
            MoveProjectile();
        }

    }
    //Update projectiles position
    void MoveProjectile()
    {
        //Create vector of how to move
        Vector3 MoveOffset = new Vector3(HorizontalMovmentAmount, VerticalMovmentAmount, 0);
        //update the position
        this.transform.position = this.transform.position + MoveOffset;

        //TODO: destroy based on dist Travled
    }
    //Called on collison with another object
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Get hit objects attack physics script
        AttackPhysicsScript attackScript = collider.gameObject.GetComponent<AttackPhysicsScript>();
        if (collider.gameObject.GetComponent<AttackPhysicsScript>() != null)
        {
            //Hit the object it it colliding with doing damage and knockback
            attackScript.OnHit(DamageAmount, AttackStrength, this.transform.position);
            //Update total damge stat on player that made projectile
            OwningClassScript.AddTotalDamage(DamageAmount *  AttackStrength);
            //Destory the Projectile
            Destroy(this.gameObject);
        }
    }
}
