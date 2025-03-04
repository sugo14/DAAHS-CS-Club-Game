using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Fall,
    Dash,
    SideMelee,
    UpMelee,
    DownMelee,
    SideSpecial,
    UpSpecial,
    DownSpecial
}

public class PlayerStateScript : MonoBehaviour
{
    public bool facingRight;
    public PlayerMovementScript playerMovement;
    public AttackPhysicsScript attackPhysics;
    public PlayerState playerState;

    void UpdateState()
    {
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

        if (vel.x != 0)
        {
            facingRight = Mathf.Sign(vel.x) == 1;
        }

        if (vel.y > 0)
        {
            playerState = PlayerState.Jump;
        }
        else if (vel.y < 0)
        {
            playerState = PlayerState.Fall;
        }
        else if (vel.x != 0)
        {
            playerState = PlayerState.Run;
        }
        else
        {
            playerState = PlayerState.Idle;
        }
    }

    void Update()
    {
        UpdateState();
    }
}
