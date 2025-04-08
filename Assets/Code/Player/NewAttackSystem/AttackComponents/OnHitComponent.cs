using UnityEngine;

public class OnHitComponent : AttackComponent
{
    [SerializeField] LayerMask[] hitLayers;
    [SerializeField] AttackComponent[] componentsToEnable;
    [SerializeField] HitboxProfile[] hitboxesToEnable;

    public void OnHitEvent(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (AttackComponent component in componentsToEnable)
                {
                    component.enabled = true;
                    component.Initialize(owningAttack);
                }
                foreach (HitboxProfile hitbox in hitboxesToEnable)
                {
                    hitbox.enabled = true;
                }
                return;
            }
        }
    }
}
