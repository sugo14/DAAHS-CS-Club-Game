using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AresClass : ClassBaseScript
{

    bool LeftRight;

    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    public bool DevDoVerticalAttack;
    bool DevDoVerticalAttackOld;
    public bool DevDownUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Dev testing side attack
        if (DevDoSideAttack != DevDoSideAttackOld)
        {
            AttackSide(DevLeftRight);
            // Save the current state of the check box
            DevDoSideAttackOld = DevDoSideAttack;
        }
        if (DevDoVerticalAttack != DevDoVerticalAttackOld)
        {
            if (DevDownUp)
            {
                AttackUp();
            }
            else
            {
                AttackDown();
            }
            // Save the current state of the check box
            DevDoVerticalAttackOld = DevDoVerticalAttack;
        }

        if (IsChargingAttackSide && (ChargeTime >= SideAttackChargeTime))
        {
            SideSword();

        }
    }
     public override void AttackSide(bool InLeftRight)
    {
        UnityEngine.Debug.Log("DevMelee AttackSide");
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackSide = true;
            LeftRight = InLeftRight;
            UnityEngine.Debug.Log("Charging AttackSide");

        }
    }
    public override void AttackUp()
    {
        UnityEngine.Debug.Log("DevMelee AttackUp");
    }
    public override void AttackDown()
    {
        UnityEngine.Debug.Log("DevMelee AttackDown");
    }

    private void SideSword()
    {
        UnityEngine.Debug.Log("Side Sword");
    }
}
