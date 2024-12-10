using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DevRangedAttack : AttackScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     public override void AttackSide()
    {
        UnityEngine.Debug.Log("DevRanged AttackSide");
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
