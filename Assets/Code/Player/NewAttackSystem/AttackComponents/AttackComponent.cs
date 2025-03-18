using UnityEngine;

/// <summary>
/// An abstract class that all attack components inherit from.
/// </summary>
public abstract class AttackComponent : MonoBehaviour
{
    protected Attack owningAttack;

    public virtual void Initialize(Attack owningAttack, Facing facing)
    {
        this.owningAttack = owningAttack;
    }
}
