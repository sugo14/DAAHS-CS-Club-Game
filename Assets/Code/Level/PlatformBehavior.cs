using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    // References
    public SpriteRenderer SR;
    public BoxCollider2D BoxCollider;

    // Preferences 
    public bool isPassthrough = false;

    void Awake()
    {
        // Setting properties
        if (isPassthrough)
        {
            BoxCollider.usedByEffector = true;
            transform.gameObject.tag = "Passthrough";
        }
        else
        {
            BoxCollider.usedByEffector = false;
            transform.gameObject.tag = "Untagged";
        }
    }
}
