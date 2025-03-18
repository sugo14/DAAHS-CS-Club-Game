using UnityEngine;

/// <summary>
/// Controls the behavior of platforms, including color and passthrough properties.
/// </summary>
public class PlatformBehavior : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] bool isPassthrough = false;

    public void SetColor(Color color)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = color;
        }
    }

    void Awake()
    {
        // Setting properties
        if (isPassthrough)
        {
            boxCollider.usedByEffector = true;
            transform.gameObject.tag = "Passthrough";
        }
        else
        {
            boxCollider.usedByEffector = false;
            transform.gameObject.tag = "Untagged";
        }
    }
}
