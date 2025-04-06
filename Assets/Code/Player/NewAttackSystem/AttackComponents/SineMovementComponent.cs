using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovementComponent : MovementComponent
{
    [SerializeField] Vector2 amplitude = new Vector2(0, 1);
    [SerializeField] Vector2 frequency = new Vector2(1, 1);
    [SerializeField] Vector2 addend = new Vector2(0, 0);

    private float frames = 0, mult = 1 / 60f;

    public override void Initialize(Attack owningAttack)
    {
        base.Initialize(owningAttack);

        if (owningAttack.FacingDirection == Facing.Left)
        {
            amplitude.x = -amplitude.x;
        }
    }

    void Update()
    {
        frames++;
        float x = amplitude.x * Mathf.Sin(frequency.x * frames * mult) + addend.x;
        float y = amplitude.y * Mathf.Sin(frequency.y * frames * mult) + addend.y;
        SetCurrentDisplacement(new Vector2(x, y));
    }
}
