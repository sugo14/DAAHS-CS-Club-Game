using UnityEngine;

/// <summary>
/// An attack component that modifies the movement of the projectile to ensure multiple movements work together as intended.
/// </summary>
public class MovementComponent : AttackComponent
{
    Vector2 lastDisplacement = new Vector2(0, 0);

    /// <summary>
    /// Sets the displacement of the projectile in one component, in such a way that multiple movement components can work together.
    /// </summary>
    /// <param name="displacement"></param>
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
