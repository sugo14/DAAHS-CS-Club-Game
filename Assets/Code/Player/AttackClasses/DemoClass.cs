using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoClass : ClassBase
{
    // Start is called before the first frame update
    public bool DevDoSideAttack;
    bool DevDoSideAttackOld;
    public bool DevLeftRight;
    public bool DevDoVerticalAttack;
    bool DevDoVerticalAttackOld;
    public bool DevDownUp;
    //Refrence to projectile class to spawn
    public GameObject ProjectileClass;

    public float HorizontalDistance = 25;
    public float VerticalDistance = 25;

    public float HorizontalSpeed = 0.25f;
    public float UpProjectileSpeed = 0.25f;
    public float DownProjectileSpeed = 0.2f;

public GameObject swordClass;

    bool LeftRight;
    public bool MeleeOrRanged;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //Dev testing side attack
        if (DevDoSideAttack != DevDoSideAttackOld)
        {
            DemoSide(DevLeftRight, MeleeOrRanged);
            // Save the current state of the check box
            DevDoSideAttackOld = DevDoSideAttack;
        }
        if (DevDoVerticalAttack != DevDoVerticalAttackOld)
        {
            if (DevDownUp)
            {
                DemoUp(MeleeOrRanged);
            }
            else
            {
                DemoDown(MeleeOrRanged);
            }
            // Save the current state of the check box
            DevDoVerticalAttackOld = DevDoVerticalAttack;
        }
        if (IsChargingAttackSide || IsChargingAttackDown || IsChargingAttackUp)
        {
            ChargeTime += Time.deltaTime;
        }
        if (IsChargingAttackSide && (ChargeTime >= SideAttackChargeTime))
        {
            if (MeleeOrRanged)
            {
                ShootSide();
            }
            else
            {
                SideSword();
            }

        }
        if (IsChargingAttackUp && (ChargeTime >= UpAttackChargeTime))
        {
            if (MeleeOrRanged)
            {
                ShootUp();
            }
            else
            {
                UpSword();
            }

        }
        if (IsChargingAttackDown && (ChargeTime >= DownAttackChargeTime))
        {
            if (MeleeOrRanged)
            {
                ShootDown();
            }
            else
            {
                DownSword();
            }

        }
    }
    public void DemoSide(bool InLeftRight, bool MeleeOrRangedIn)
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackSide = true;
            LeftRight = InLeftRight;
            MeleeOrRanged = MeleeOrRangedIn;
        }
    }
    public void DemoUp(bool MeleeOrRangedIn)
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackUp = true;
            MeleeOrRanged = MeleeOrRangedIn;
        }
    }
    public void DemoDown(bool MeleeOrRangedIn)
    {
        if (!IsChargingAttackSide || !IsChargingAttackDown || !IsChargingAttackUp)
        {
            IsChargingAttackDown = true;
            MeleeOrRanged = MeleeOrRangedIn;
        }
    }

    void ShootSide()
    {
        AudioManager.PlaySound("Gun1");
        //Creates a projectile and set all its releavent vars
        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
        projectileObject.transform.position = transform.position;
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();

        projectileScript.SetUp(this, 5, 1, HorizontalDistance, LeftRight, HorizontalSpeed);
        ResetCharge();
    }
    void ShootUp()
    {
        AudioManager.PlaySound("Gun1");
        //Creates a projectile and set all its releavent vars
        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
        projectileObject.transform.position = this.transform.position;
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.SetUp(this, 5, 1, VerticalDistance, false, 0, true, UpProjectileSpeed);
        ResetCharge();
        //TODO Vertical Boost
    }
    void ShootDown()
    {
        AudioManager.PlaySound("Gun1");
        //Creates a projectile and set all its releavent vars
        GameObject projectileObject = Instantiate(ProjectileClass, this.transform.position, Quaternion.identity);
        projectileObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
        projectileObject.transform.position = this.transform.position;
        Projectile projectileScript = projectileObject.GetComponent<Projectile>();

        projectileScript.SetUp(this, 8, 1, VerticalDistance, false, 0, false, DownProjectileSpeed);
        ResetCharge();
    }
    void SideSword()
    {
        AudioManager.PlaySound("Sword1");
        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
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
        AudioManager.PlaySound("Sword1");
        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
        swordObject.transform.SetParent(this.transform);
        Sword swordScript = swordObject.GetComponent<Sword>();
        swordObject.transform.localPosition = new Vector2(0, 0);
        swordScript.SetUp(this, new Vector2(0, 3), 10, 10, 1);

        ResetCharge();
    }
    void DownSword()
    {
        AudioManager.PlaySound("Sword1");
        GameObject swordObject = Instantiate(swordClass, this.transform.position, Quaternion.identity);
        swordObject.GetComponent<SpriteRenderer>().color = GetComponent<AttackPhysics>().playerSplash.backdropColor;
        swordObject.transform.SetParent(this.transform);
        Sword swordScript = swordObject.GetComponent<Sword>();
        swordObject.transform.localPosition = new Vector2(.5f, -1);
        swordObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        swordScript.SetUp(this, new Vector2(-.5f, -1), 10, 15, 1.5f);

        ResetCharge();
    }
    
    public override void AttackSide(bool InLeftRight){
        UnityEngine.Debug.Log("YOU SOULDNT BE HERE IN THIS SIDE ATTACK");
    }
     public override void AttackUp(){
        UnityEngine.Debug.Log("YOU SOULDNT BE HERE IN THIS UP ATTACK");
    }
    public override void AttackDown(){
        UnityEngine.Debug.Log("YOU SOULDNT BE HERE IN THIS Down ATTACK");
    }
}
