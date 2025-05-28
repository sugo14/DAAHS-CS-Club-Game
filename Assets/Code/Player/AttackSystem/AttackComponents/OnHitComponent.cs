using System.Collections;
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
    [SerializeField] GameObject[] objectsToDisable;

    public void EnableComponents(Collider2D collider)
    {
        StartCoroutine(EnableComponentsCoroutine(collider));
    }

    public void DisableComponents(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (AttackComponent component in componentsToDisable)
                {
                    Destroy(component);
                    continue;
                    component.enabled = false;
                    component.gameObject.SetActive(false);
                }
                return;
            }
        }
    }

    IEnumerator EnableComponentsCoroutine(Collider2D collider)
    {
        yield return new WaitForEndOfFrame();
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

    public void DisableObjects(Collider2D collider)
    {
        foreach (LayerMask layer in hitLayers)
        {
            if (layer == (layer | (1 << collider.gameObject.layer)))
            {
                foreach (GameObject obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
                return;
            }
        }
    }
}
