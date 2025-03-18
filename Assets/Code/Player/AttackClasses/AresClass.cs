using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AresClass : ClassBase
{

    bool LeftRight;

    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    public bool DevDoVerticalAttack;
    bool DevDoVerticalAttackOld;
    public bool DevDownUp;

    public GameObject swordClass;
    
    protected override void Update()
    {
        base.Update();

        // Dev testing side attack
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
        if (IsChargingAttackUp && (ChargeTime >= UpAttackChargeTime))
        {
            UpSword();

        }
        if (IsChargingAttackDown && (ChargeTime >= DownAttackChargeTime))
        {
            DownSword();

        }
    }
    public override void AttackSide(bool InLeftRight)
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackSide = true;
            LeftRight = InLeftRight;

        }
    }
    public override void AttackUp()
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackUp = true;


        };
    }
    public override void AttackDown()
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackDown = true;

        }
    }

    void SideSword()
    {

        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.transform.SetParent(this.transform);
        Sword swordScript = swordObject.GetComponent<Sword>();
        if (!LeftRight)
        {
            swordObject.transform.localPosition = new Vector2(0f, 0f);
            swordObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            swordScript.SetUp(this, new Vector2(2f, 0f), 8, 10, 1);
        }
        else
        {
            swordObject.transform.localPosition = new Vector2(0f, 0f);
            swordObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            swordScript.SetUp(this, new Vector2(-2f, 0f), 8, 10, 1);
        }



        ResetCharge();
    }
    void UpSword()
    {
        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.transform.SetParent(this.transform);
        Sword swordScript = swordObject.GetComponent<Sword>();
        swordObject.transform.localPosition = new Vector2(0, 0);
        swordScript.SetUp(this, new Vector2(0, 3), 10, 15, 1);

        ResetCharge();
    }
    void DownSword()
    {
        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.transform.SetParent(this.transform);
        Sword swordScript = swordObject.GetComponent<Sword>();
        swordObject.transform.localPosition = new Vector2(.5f, -1);
        swordObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        swordScript.SetUp(this, new Vector2(-.5f, -1), 10, 5, 1.5f);

        ResetCharge();
    }
}
