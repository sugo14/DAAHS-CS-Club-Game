using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public bool LeftOrRight;
    public bool StartMoving;
    public float HorizontalMovmentAmount = 1;
    public float VerticalMovmentAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartMoving == true){
            Vector3 MoveOffset = new Vector3(HorizontalMovmentAmount, VerticalMovmentAmount, 0);
            if(LeftOrRight == false){
                MoveOffset.x = MoveOffset.x * -1;
            }
            this.transform.position = this.transform.position + MoveOffset; 
        }
    }
}
