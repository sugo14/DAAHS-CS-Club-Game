using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // References
    public SpriteRenderer SR;
    public BoxCollider2D BoxCollider;

    // Preferences 
    public bool isPassthrough = false;

    void Awake()
    {
        // setting proproties
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
