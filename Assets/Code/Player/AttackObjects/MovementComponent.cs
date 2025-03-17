using UnityEngine;

// An attack component that modifies the movement of the projectile
// To ensure multiple movements work together as intended, this class is the only one that should modify the position of the projectile
public class MovementComponent : AttackComponent
{
    Vector2 lastDisplacement = new Vector2(0, 0);

    protected void SetCurrentDisplacement(Vector2 displacement)
    {
        transform.position -= (Vector3)lastDisplacement;
        transform.position += (Vector3)displacement;

        lastDisplacement = displacement;
    }

    protected void AddMovement(Vector2 velocity)
    {
        SetCurrentDisplacement(lastDisplacement + velocity);
    }
}
