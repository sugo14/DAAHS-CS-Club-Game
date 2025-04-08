using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main script on an attack object. Manages attack lifetime, components, and hitboxes.
/// </summary>
public class Attack : MonoBehaviour
{
    public GameObject OwningPlayer { get; private set; }
    public Facing FacingDirection { get; private set; }
    // TODO: What was this for?
    public Vector2 AttackOrigin { get; private set; }

    [SerializeField] float StalingFactor = 0.75f;
    [SerializeField] int Lifetime = 60;

    HashSet<GameObject> HitObjects;
    List<AttackComponent> attackComponents;
    int lifetimeTimer;

    /// <summary>
    /// Initializes all attack components and hitboxes. Used within the Attack class, and also by AttackComponents that initialize other AttackComponents.
    /// </summary>
    public static void InitializeComponents(List<AttackComponent> attackComponents, Attack owningAttack, bool onAttackBegin = false)
    {
        foreach (AttackComponent attackComponent in attackComponents)
        {
            if (!(!attackComponent.initializeOnAttackBegin && onAttackBegin))
            {
                attackComponent.gameObject.SetActive(true);
                attackComponent.Initialize(owningAttack);
            }
        }
    }

    public void Initialize(GameObject player, Facing facing)
    {
        OwningPlayer = player;
        FacingDirection = facing;

        // Set up attack components and hitboxes
        attackComponents = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>());
        InitializeComponents(attackComponents, this, true);

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

    public void AddHitObject(GameObject hitObject)
    {
        HitObjects.Add(hitObject);
    }
    public bool HasHitObject(GameObject hitObject)
    {
        return HitObjects.Contains(hitObject);
    }
    public void RemoveHitObject(GameObject hitObject)
    {
        if (!HitObjects.Contains(hitObject))
        {
            Debug.LogError("Warning: Hit object not found in hit objects list.");
            return;
        }
        HitObjects.Remove(hitObject);
    }

    void Start()
    {
        HitObjects = new HashSet<GameObject>();
        AttackOrigin = transform.position;
    }

    void Update()
    {
        lifetimeTimer--;
        if (lifetimeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
