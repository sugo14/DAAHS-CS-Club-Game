using System;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using UnityEngine;


public class AttackPhysicsScript : MonoBehaviour
{
    //The equivlent to your % in Smash Bros
    public float Damage;

    //Debuging for damage
    public bool DevDoImpulse;
    bool DevDoImpulseOld;
    public float DevDamagePerHitAmount = 0;

    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get a refrence to the charcters RigidBody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Dev testing the Impulse funcionality
        //If the impulse checkbox has changed do an impulse
        if (DevDoImpulse!=DevDoImpulseOld) 
        {
            OnHit(DevDamagePerHitAmount, 1 ,new Vector2(0, -1.5f));
            //Save the current state of the check box
            DevDoImpulseOld = DevDoImpulse;

        }
    }

    //Function for when somthing gets hit
    //HitDamge is the amount to increase the damage by
    //AttackSrength is a multiplier for if there a weak or strong attacks such as crits
    //AttackFromPos is where the attacker is so we can find the direction to move the player
    void OnHit(float HitDamage, float AttackStrength ,Vector2 AttackedFromPos)
    {

        if (rb != null)
        {
            //Create variable to store which way the charecter is pushed
            Vector2 Dir = new Vector2(0,0);

            //Find which way to push the charecter
            if(AttackedFromPos.x > transform.position.x)
            {
                Dir.x = -1;
            }
            else
            {
                Dir.x = 1;
            }

            //Increase the Damage
            Damage = Damage + HitDamage;

            //Set the knockback values
            //Horizontal is either positive or negative dpending on Dir and the damage multiplied by attack strength
            float horizontalForce = Dir.x * Damage * AttackStrength;
            //Vertical is the damage multiplied by attack strength but divide so the motion is primaily hoizontal
            float verticalForce = (Damage * AttackStrength) / 4;

            //2 ways to do the knockback
            rb.AddForce(new Vector2(horizontalForce*100, verticalForce*100), ForceMode2D.Impulse);

            //rb.linearVelocity = new Vector2(horizontalForce, verticalForce);

           


        }
    }

}