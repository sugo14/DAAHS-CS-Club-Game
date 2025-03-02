using UnityEngine;
using UnityEngine.UI;
using TMPro;
 
public class AttackPhysicsScript : MonoBehaviour
{
    public PlayerSplashScript playerSplashScript;

    // The equivlent to your % in Smash Bros
    public float Damage;

    // Debugging for damage
    public bool DevDoImpulse;
    bool DevDoImpulseOld;
    public float DevDamagePerHitAmount = 0;

    public bool IsInvulnerable;

    private Rigidbody2D RB;
    private ClassBaseScript ClassScript;

    void Start()
    {
        // Get a refrence to the charcters RigidBody
        RB = GetComponent<Rigidbody2D>();
        ClassScript = GetComponent<ClassBaseScript>();

        Damage = 10;
    }

    void Update()
    {
        // Dev testing the Impulse funcionality
        // If the impulse checkbox has changed do an impulse
        if (DevDoImpulse != DevDoImpulseOld)
        {
            OnHit(DevDamagePerHitAmount, 1, new Vector2(0, -1.5f));
            // Save the current state of the check box
            DevDoImpulseOld = DevDoImpulse;
        }
    }

    // Function for when somthing gets hit
    // HitDamge is the amount to increase the damage by
    // AttackSrength is a multiplier for if there a weak or strong attacks such as crits
    // AttackFromPos is where the player is hit from such as another player or projectile
    public void OnHit(float HitDamage, float AttackStrength, Vector2 AttackedFromPos)
    {
        if (RB != null && IsInvulnerable == false)
        {
            // Create variable to store which way the character is pushed
            Vector2 dir = new Vector2(0,0);

            //OLD Dir.x Calcualtion
            // Find which way to push the character
            // Always 1 so that significant horizontal knockback is always applied
            //if(AttackedFromPos.x > transform.position.x)
            //{
            //    dir.x = -1;
            //}
            //else
            //{
            //    dir.x = 1;
            //}

            dir.x = (this.transform.position.x+20)  - (AttackedFromPos.x+20);
            Debug.Log(dir.x);
            dir.y = this.transform.position.y - AttackedFromPos.y + 0.5f;
            Debug.Log(dir.y);
            // Increase the Damage
            Damage = Damage + HitDamage;

            // Set the knockback values
            // Horizontal is either positive or negative dpending on Dir and the damage multiplied by attack strength
            float horizontalForce = dir.x * Damage * AttackStrength;
            // Vertical is the damage multiplied by attack strength
            float verticalForce = (dir.y * Damage * AttackStrength);

            if(ClassScript != null)
            {
                ClassScript.ResetCharge();
            }

            RB.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);
            //Update Precent text
            if(playerSplashScript != null) 
            {
                playerSplashScript.SetPercent((int)Damage - 10);
            }  
        }
    }
}

