using UnityEngine;

/// <summary>
/// States that the player can be in.
/// </summary>
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

/// <summary>
/// Manages the state of the player character for animations and sounds.
/// </summary>
public class PlayerStateManager : MonoBehaviour
{
    public bool facingRight;
    public PlayerMovement playerMovement;
    public AttackPhysics attackPhysics;
    public GameObject srObject;
    public Animator animator;
    public Sprite idleSprite, fallingSprite, jumpSprite;
    public PlayerState playerState;

    void FixedUpdate()
    {
        UpdateState();
        UpdateAnimator();
    }

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

        srObject.GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        bool useStaticSprite = playerState == PlayerState.Idle || playerState == PlayerState.Fall || playerState == PlayerState.Jump;

        if (useStaticSprite)
        {
            // Disable Animator for static sprites
            animator.enabled = false;

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
            // Re-enable when needed
            if (!animator.enabled) animator.enabled = true;

            animator.SetBool("isRunning", playerState == PlayerState.Run);
            animator.SetBool("isJumping", playerState == PlayerState.Jump);
            animator.SetBool("isFalling", playerState == PlayerState.Fall);
            animator.SetBool("isDashing", playerState == PlayerState.Dash);
        }
    }
}
