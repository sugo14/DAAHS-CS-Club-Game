using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public bool LeftOrRight;
    public bool IsMoving;
    public float HorizontalMovmentAmount = 1;
    public float VerticalMovmentAmount = 0;
    public AttackScript OwningAttackScript;
    private BoxCollider2D projectileCollider;
    private BoxCollider2D ownerCollider;


    // Start is called before the first frame update
    void Start()
    {

        ownerCollider = OwningAttackScript.GetComponent<BoxCollider2D>();
        if (OwningAttackScript != null)
        {
            projectileCollider = GetComponent<BoxCollider2D>();
        }
        if (projectileCollider != null && ownerCollider != null)
        {
                Physics2D.IgnoreCollision(projectileCollider, ownerCollider);
        }
        else
        {
            UnityEngine.Debug.LogError("Projecile Collider or owner Collider is NUll");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (IsMoving == true)
        {
            MoveProjectile();
        }

    }
    void MoveProjectile()
    {
        Vector3 MoveOffset = new Vector3(HorizontalMovmentAmount, VerticalMovmentAmount, 0);
        if (LeftOrRight == false)
        {
            MoveOffset.x = MoveOffset.x * -1;
        }
        this.transform.position = this.transform.position + MoveOffset;
    }

}
