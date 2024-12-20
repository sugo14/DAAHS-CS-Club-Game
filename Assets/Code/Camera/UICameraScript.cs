using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraScript : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.depth = -10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
