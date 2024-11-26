using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttackScript : MonoBehaviour
{
    public float attackRange = 1.0f;                 
    public LayerMask enemyLayer;              
    public float attackCooldown = 0.5f;       
    private float lastAttackTime = 0;

    public Transform attackPoint;
    // when J is pressed, player attack
    void Update()
    {
       
    }
    //player attack
    public void Attack(InputAction.CallbackContext context)
    {

        // GetComponent<Animator>().SetTrigger("Attack");
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        
        foreach (Collider2D enemy in hitEnemies)
        {
          
            enemy.GetComponent<Enemy>().TakeDamage(HitDamage);
        }

        
        Debug.Log("Attack hit " + hitEnemies.Length + " enemies!");
    }
    //draw the attack range
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
