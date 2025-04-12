using UnityEngine;

/// <summary>
/// Contains functions that can be added to a Hitbox's OnHit event.
/// Only performs functions when hitbox collides with specific layers.
/// </summary>
public class OnHitComponent : AttackComponent
{
    [SerializeField] LayerMask[] hitLayers;
    [SerializeField] AttackComponent[] componentsToEnable;
    [SerializeField] AttackComponent[] componentsToDisable;
    [SerializeField] GameObject[] objectsToEnable;

    public void EnableComponents(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (AttackComponent component in componentsToEnable)
                {
                    component.enabled = true;
                    component.gameObject.SetActive(true);
                    component.Initialize(owningAttack);
                }
                return;
            }
        }
    }

    public void DisableComponents(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (AttackComponent component in componentsToEnable)
                {
                    component.enabled = false;
                    component.gameObject.SetActive(false);
                }
                return;
            }
        }
    }

    public void EnableObjects(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (GameObject obj in objectsToEnable)
                {
                    obj.SetActive(true);
                }
                return;
            }
        }
    }
}
