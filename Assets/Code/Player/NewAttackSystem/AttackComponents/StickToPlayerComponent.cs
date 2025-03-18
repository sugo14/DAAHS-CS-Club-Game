using UnityEngine;

/// <summary>
/// Causes the attack to remain in place relative to the player.
/// </summary>
public class StickToPlayerComponent : MovementComponent
{
    Vector3 originalPosition;

    public override void Initialize(Attack owning, Facing facing)
    {
        base.Initialize(owning, facing);
        originalPosition = transform.position;
    }

    void Update()
    {
        SetCurrentDisplacement(owningAttack.OwningPlayer.transform.position - originalPosition);
    }
}
