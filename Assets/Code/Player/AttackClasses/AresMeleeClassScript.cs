using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AresClass : AttackScript
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
        UnityEngine.Debug.Log("DevMelee AttackSide");
    }
    public override void AttackUp()
    {
        UnityEngine.Debug.Log("DevMelee AttackUp");
    }
    public override void AttackDown()
    {
        UnityEngine.Debug.Log("DevMelee AttackDown");
    }
}
