using UnityEngine;

public class ProjectileComponent : MovementComponent
{
    // Set to true after all other vars are set to make sure it can collide before it fully set up
    public bool IsMoving = false;
    public Vector2 InitialVelocity = new Vector2(1, 0);
    public Vector2 Acceleration = new Vector2(0, 0);
    
    Vector2 currentVelocity = new Vector2(0, 0);

    public override void Initialize(Attack owningAttack)
    {
        base.Initialize(owningAttack);
        IsMoving = true;
    }

    void Update()
    {
        if (IsMoving == true)
        {
            MoveProjectile();
        }
    }

    // Update projectiles position
    void MoveProjectile()
    {
        AddMovement(currentVelocity);
        currentVelocity += Acceleration;
    }
}
