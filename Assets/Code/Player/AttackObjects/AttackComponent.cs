using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public Attack owningAttack;

    public virtual void Initialize(Attack owningAttack)
    {
        this.owningAttack = owningAttack;
    }
}
