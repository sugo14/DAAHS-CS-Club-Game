using System.Collections.Generic;
using UnityEngine;

public struct LagProfile
{
    public float frames;
    public bool canChangeMomentum;
    public bool canJump;
    public bool becomesStationary;
}

public class Attack : MonoBehaviour
{
    public List<AttackComponent> attackComponents;
    public List<HitboxProfile> hitboxes;
    public List<LagProfile> lags;
    public ClassBase OwningPlayer { get; private set; }

    public Vector2 AttackOrigin { get; private set; }

    public float StalingFactor = 0.75f;
    public int Lifetime = 60;

    int lifetimeTimer;

    public void Initialize(GameObject player)
    {
        OwningPlayer = player.GetComponent<ClassBase>();
        foreach (AttackComponent attackComponent in attackComponents)
        {
            attackComponent.Initialize(this);
        }
        foreach (HitboxProfile hitbox in hitboxes)
        {
            hitbox.Initialize(this);
        }

        BoxCollider2D ownerCollider = OwningPlayer.GetComponent<BoxCollider2D>();
        foreach (HitboxProfile hitbox in hitboxes)
        {
            Physics2D.IgnoreCollision(hitbox.hitbox, ownerCollider);
        }

        lifetimeTimer = Lifetime;
    }

    public void Update()
    {
        lifetimeTimer--;
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
