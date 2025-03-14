using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxScript : MonoBehaviour
{
    public int HitDamage = 5;
    public float AttackStrength = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.gameObject.tag == "Player")
        //{
        //    Debug.Log("Player hit!");
        //    AttackPhysicsScript player = collider.gameObject.GetComponent<AttackPhysicsScript>();
        //    player.OnHit(HitDamage, AttackStrength, this.transform.position);
        //}
        if (collider.gameObject.GetComponent<AttackPhysicsScript>() != null)
        {
            Debug.Log("Attack Object hit!");
            AttackPhysicsScript player = collider.gameObject.GetComponent<AttackPhysicsScript>();
            player.OnHit(HitDamage, AttackStrength, this.transform.position);
        }
    }
}
