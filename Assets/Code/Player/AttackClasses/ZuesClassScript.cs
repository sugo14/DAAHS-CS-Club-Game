using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
//using ProjectileScript;

public class ZuesClass : ClassBaseScript
{
    //Dev Testing vars
    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    //Refrence to projectile class to spawn
    public GameObject ProjectileClass;

    // Update is called once per frame
    void Update()
    {
        //Dev testing side attack
        if (DevDoSideAttack!=DevDoSideAttackOld) 
        {
            AttackSide();
            // Save the current state of the check box
            DevDoSideAttackOld = DevDoSideAttack;
        }
    }
    public override void AttackSide()
    {
        //TODO: Move to a function in the Projectile Script
        //Creates a projectile and set all its releavent vars
        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.transform.position = this.transform.position;
        ProjectileScript projectileScript = projectileObject.GetComponent<ProjectileScript>();
        if (DevLeftRight)
        {
            projectileScript.HorizontalMovmentAmount = -0.25F;
        }
        else
        {
            projectileScript.HorizontalMovmentAmount = 0.25F;
        }

        projectileScript.OwningClassScript = this;
        projectileScript.DamageAmount = 5;

        projectileScript.IsMoving = true;

    }
    public override void AttackUp()
    {
        UnityEngine.Debug.Log("DevRanged AttackUp");
    }
    public override void AttackDown()
    {
        UnityEngine.Debug.Log("DevRanged AttackDown");
    }
}
