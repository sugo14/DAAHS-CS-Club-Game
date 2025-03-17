using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public int HitDamage = 5;
    public float AttackStrength = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<AttackPhysics>() != null)
        {
            Debug.Log("Attack Object hit!");
            AttackPhysics player = collider.gameObject.GetComponent<AttackPhysics>();
            player.OnHit(HitDamage, AttackStrength, this.transform.position);
        }
    }
}
