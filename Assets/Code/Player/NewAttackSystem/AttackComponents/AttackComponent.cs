using UnityEngine;

/// <summary>
/// An abstract class that all attack components inherit from.
/// Adds unique functionality to attacks.
/// Derived classes should call base.Initialize() in their own Initialize() method.
/// </summary>
public abstract class AttackComponent : MonoBehaviour
{
    public bool initializeOnAttackBegin = true;

    protected Attack owningAttack;

    public virtual void Initialize(Attack owningAttack)
    {
        this.owningAttack = owningAttack;
    }
}
