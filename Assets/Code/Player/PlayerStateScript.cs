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
    public GameObject srObject;
    public Animator animator;
    public Sprite idleSprite, fallingSprite, jumpSprite;
    public PlayerState playerState;

    void UpdateState()
    {
        Vector2 vel = GetComponent<Rigidbody2D>().velocity;
        float small = 0f;

        if (vel.x > small || vel.x < -small)
        {
            facingRight = Mathf.Sign(vel.x) == 1;
        }

        if (vel.y > small)
        {
            playerState = PlayerState.Jump;
        }
        else if (vel.y < -small)
        {
            playerState = PlayerState.Fall;
        }
        else if (vel.x > small || vel.x < -small)
        {
            playerState = PlayerState.Run;
        }
        else
        {
            playerState = PlayerState.Idle;
        }

        Debug.Log(playerState.ToString());

        srObject.GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool useStaticSprite = playerState == PlayerState.Idle || playerState == PlayerState.Fall || playerState == PlayerState.Jump;

        if (useStaticSprite)
        {
            animator.enabled = false; // Disable Animator for static sprites

            if (playerState == PlayerState.Idle)
            {
                srObject.GetComponent<SpriteRenderer>().sprite = idleSprite;
            }
            else if (playerState == PlayerState.Fall)
            {
                srObject.GetComponent<SpriteRenderer>().sprite = fallingSprite;
            }
            else if (playerState == PlayerState.Jump)
            {
                srObject.GetComponent<SpriteRenderer>().sprite = jumpSprite;
            }
        }
        else
        {
            if (!animator.enabled) animator.enabled = true; // Re-enable when needed

            animator.SetBool("isRunning", playerState == PlayerState.Run);
            animator.SetBool("isJumping", playerState == PlayerState.Jump);
            animator.SetBool("isFalling", playerState == PlayerState.Fall);
            animator.SetBool("isDashing", playerState == PlayerState.Dash);
        }
    }


    void Update()
    {
        UpdateState();
        UpdateAnimator();
    }
}
