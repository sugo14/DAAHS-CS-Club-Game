using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class ClassBaseScript : MonoBehaviour
{
    //Side attack function
    public abstract void AttackSide(bool InLeftRight);
    //Down attack function
    public abstract void AttackDown();
    //Up attack function
    public abstract void AttackUp();

    //The sprite to use for the class
    public Sprite PlayerSprite;
    //Stat of the total damage done, NOT used for anything currently
    public float TotalDamageDelt;

    protected bool IsChargingAttackSide;
    protected bool IsChargingAttackDown;
    protected bool IsChargingAttackUp;

    public float SideAttackChargeTime = 1;
    public float UpAttackChargeTime = 1;
    public float DownAttackChargeTime = 1;
    protected float ChargeTime;


    // Start is called before the first frame update
    void Start()
    {
        //Get sprite renderer child of player
        //TODO move sprite render and component on player instead of child, make sprite render not streched.
        /* this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = PlayerSprite; */


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsChargingAttackSide || IsChargingAttackDown || IsChargingAttackUp)
        {
            ChargeTime += Time.deltaTime;
        }
    }

    //Add to the total Damage Stat
    public void AddTotalDamage(float DamageDone)
    {
        TotalDamageDelt += DamageDone;
    }

    public void ResetCharge()
    {
        IsChargingAttackSide = false;
        IsChargingAttackDown = false;
        IsChargingAttackUp = false;
        ChargeTime = 0;
    }



}

