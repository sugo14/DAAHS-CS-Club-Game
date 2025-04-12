using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerComponent : AttackComponent
{
    [SerializeField] Vector2 setVelocity = new Vector2(0, 0);

    override public void Initialize(Attack attack)
    {
        base.Initialize(attack);
        attack.OwningPlayer.GetComponent<Rigidbody2D>().velocity = setVelocity;
    }
}
