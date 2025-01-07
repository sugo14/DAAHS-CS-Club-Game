using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
//using ProjectileScript;

public class ZuesClass : AttackScript
{

    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    public GameObject ProjectileClass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DevDoSideAttack!=DevDoSideAttackOld) 
        {
            AttackSide();
            // Save the current state of the check box
            DevDoSideAttackOld = DevDoSideAttack;
        }
    }
    public override void AttackSide()
    {
        UnityEngine.Debug.Log("DevRanged AttackSide");


        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.transform.position = this.transform.position;
        ProjectileScript projectileScript = projectileObject.GetComponent<ProjectileScript>();
        projectileScript.HorizontalMovmentAmount = 0.25F;
        projectileScript.LeftOrRight = DevLeftRight;
        projectileScript.StartMoving = true;



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
