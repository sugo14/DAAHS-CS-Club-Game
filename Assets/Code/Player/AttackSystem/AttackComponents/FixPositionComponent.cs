using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class FixPositionComponent : AttackComponent
{
    [SerializeField] Rigidbody2D rb;

    Vector3 position = new Vector3(0, 0, 0);

    public override void Initialize(Attack owningAttack)
    {
        base.Initialize(owningAttack);

        position = transform.position;
    }

    public void Update()
    {
        if (position == new Vector3(0, 0, 0))
        {
            return;
        }
        Debug.Log("Fix position to " + position);

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        transform.position = position;

        Debug.Log("Update position to " + position);
    }
}
