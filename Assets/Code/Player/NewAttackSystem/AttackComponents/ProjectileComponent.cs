using UnityEngine;

/// <summary>
/// Moves a projectile with a constant velocity and acceleration.
/// </summary>
public class ProjectileComponent : MovementComponent
{
    [SerializeField] Vector2 InitialVelocity = new Vector2(1, 0);
    [SerializeField] Vector2 Acceleration = new Vector2(0, 0);
    
    // Set to true after all other vars are set to make sure it can collide before it fully set up
    // Is this actually needed?
    bool IsMoving = false;
    Vector2 currentVelocity = new Vector2(0, 0);

    void Update()
    {
        if (IsMoving == true)
        {
            MoveProjectile();
        }
    }

    public override void Initialize(Attack owningAttack, Facing facing)
    {
        base.Initialize(owningAttack, facing);

        currentVelocity = InitialVelocity;
        if (facing == Facing.Right)
        {
            currentVelocity.x = -currentVelocity.x;
            Acceleration.x = -Acceleration.x;
        }

        IsMoving = true;
    }

    // Does Time.timeScale work as intended here?
    void MoveProjectile()
    {
        AddMovement(currentVelocity * Time.timeScale);
        currentVelocity += Acceleration * Time.timeScale;
    }
}
