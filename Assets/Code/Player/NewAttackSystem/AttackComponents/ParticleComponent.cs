using UnityEngine;

/// <summary>
/// Sets the color of a particle system based on the player color.
/// </summary>
public class ParticleComponent : AttackComponent
{
    [SerializeField] ParticleSystem particles;

    /* [SerializeField] Color redColor = new Color(183, 30, 30);
    [SerializeField] Color blueColor = new Color(8, 140, 233); */

    public override void Initialize(Attack owningAttack)
    {
        base.Initialize(owningAttack);

        ParticleSystem.MainModule ps = particles.main;
        ps.startColor = owningAttack.OwningPlayer.GetComponent<AttackPhysics>().playerSplash.backdropColor;
    }
}
