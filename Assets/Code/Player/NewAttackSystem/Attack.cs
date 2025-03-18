using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A phase of lag resulting from an attack. Currently unused.
/// </summary>
public struct LagProfile
{
    public float frames;
    public bool canChangeMomentum;
    public bool canJump;
    public bool becomesStationary;
}

/// <summary>
/// The main script on an attack object. Manages attack lifetime, components, and hitboxes.
/// </summary>
public class Attack : MonoBehaviour
{
    public ClassBase OwningPlayer { get; private set; }
    public Vector2 AttackOrigin { get; private set; }

    [SerializeField] float StalingFactor = 0.75f;
    [SerializeField] int Lifetime = 60;
    [SerializeField] List<LagProfile> lags;

    List<AttackComponent> attackComponents;
    List<HitboxProfile> hitboxes;
    int lifetimeTimer;

    public void Initialize(GameObject player, Facing facing)
    {
        OwningPlayer = player.GetComponent<ClassBase>();

        // Set up attack components and hitboxes
        attackComponents = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>());
        hitboxes = new List<HitboxProfile>(GetComponentsInChildren<HitboxProfile>());
        foreach (AttackComponent attackComponent in attackComponents)
        {
            attackComponent.Initialize(this, facing);
        }
        foreach (HitboxProfile hitbox in hitboxes)
        {
            hitbox.Initialize(this);
        }

        // Set the color of the sprites
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = OwningPlayer.GetComponent<AttackPhysics>().playerSplash.backdropColor;
        }

        // Flip the attack if facing left
        if (facing == Facing.Left)
        {
            AttackOrigin = new Vector2(-Mathf.Abs(AttackOrigin.x), AttackOrigin.y);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
