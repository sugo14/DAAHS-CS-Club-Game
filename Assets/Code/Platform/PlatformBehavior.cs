using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // References
    public SpriteRenderer SR;
    public BoxCollider2D BoxCollider;

    // Preferences 
    public bool isPassthrough = false;


    // Start is called before the first frame update
    void Start()
    {
        // setting proproties
        if (isPassthrough)
        {
            SR.color = new Color(0.2f, 0.4f, 0.2f);
            BoxCollider.usedByEffector = true;
            transform.gameObject.tag = "Passthrough";
        }
        else
        {
            SR.color = Color.black;
            BoxCollider.usedByEffector = false;
            transform.gameObject.tag = "Untagged";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
