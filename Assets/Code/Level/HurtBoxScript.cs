using System;
using UnityEngine;

/// <summary>
/// A debugging box that damages the player when they collide with it.
/// </summary>
public class HurtBox : MonoBehaviour
{
    [SerializeField] int HitDamage = 5;
    [SerializeField] float AttackStrength = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<AttackPhysics>() != null)
        {
            Debug.Log("Attack Object hit!");
            AttackPhysics player = collider.gameObject.GetComponent<AttackPhysics>();
            player.OnHit(new HitData());
        }
    }
}
