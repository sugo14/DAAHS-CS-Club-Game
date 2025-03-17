using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class ZeusClass : ClassBase
{
    // Dev testing vars
    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    public bool DevDoVerticalAttack;
    bool DevDoVerticalAttackOld;
    public bool DevDownUp;
    // Reference to projectile class to spawn
    public GameObject ProjectileClass;

    public float HorizontalDistance = 25;
    public float VerticalDistance = 25;

    public float HorizontalSpeed = 0.25f;
    public float UpProjectileSpeed = 0.25f;
    public float DownProjectileSpeed = 0.2f;

    bool LeftRight;


    // Update is called once per frame
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
            ShootSide();
        }
        if (IsChargingAttackUp && (ChargeTime >= UpAttackChargeTime))
        {
            ShootUp();
        }
        if (IsChargingAttackDown && (ChargeTime >= DownAttackChargeTime))
        {
            ShootDown();
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
        }
    }
    public override void AttackDown()
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackDown = true;
        }
    }

    void ShootSide()
    {
        CreateProjectile().SetUp(this, 5, 1, HorizontalDistance, LeftRight, HorizontalSpeed);
        ResetCharge();
    }
    void ShootUp()
    {
        CreateProjectile().SetUp(this, 3, 1, VerticalDistance, false, 0, true, UpProjectileSpeed);
        ResetCharge();
        //TODO Vertical Boost
    }
    void ShootDown()
    {
        CreateProjectile().SetUp(this, 10, 1, VerticalDistance, false, 0, false, DownProjectileSpeed);
        ResetCharge();
    }
    Projectile CreateProjectile() {
        //Creates a projectile and set all its releavent vars
        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.transform.position = this.transform.position;
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();

        return projectileScript;
    }
}
