
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public BoxCollider2D BoxCollider;
    public Rigidbody2D RB;
    public GameObject SRObject;
    public LayerMask GroundLayer;

    public float MaxHorizontalSpeed = 8f;
    public float JumpForce = 18f;
    public float AirAcceleration = 120f;
    public float GroundAcceleration = 160f;
    public float DecelerationFactor = 0.5f;
    public int MaxDoubleJumps = 3;

    float groundedBoxHeight = 0.5f;
    int currDoubleJumps;
    float horizontalInput;
    bool grounded;

    void Start()
    {
        currDoubleJumps = MaxDoubleJumps;
    }

    // debugging function for collisions
    void Db(Collider2D collider)
    {
        Debug.Log("Collided with: " + collider.gameObject.name);
        Debug.Log("Collider Type: " + collider.GetType());
        Debug.Log("Is Trigger: " + collider.isTrigger);
        Debug.Log("Bounds: " + collider.bounds);
        Debug.Log("Offset: " + collider.offset);
        Debug.Log("Size: " + collider.bounds.size);
    }

    // check if the player is on the ground using a boxcast from the player's feet
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast
        (
            transform.position - new Vector3(0f, BoxCollider.size.y * 0.5f - groundedBoxHeight * 0.25f),
            new Vector2(BoxCollider.size.x * 0.65f, groundedBoxHeight * 0.5f),
            0f,
            Vector2.down,
            0.1f,
            GroundLayer
        );
        return hit.collider != null;
    }

    [ContextMenu("Write IsGrounded")]
    void WriteIsGrounded()
    {
        Debug.Log(IsGrounded());
    }

    void Update()
    {
        grounded = IsGrounded();
        if (grounded)
        {
            currDoubleJumps = MaxDoubleJumps;
        }
    }

    // is called at same rate as Unity physics so rigidbody physics should be done here
    void FixedUpdate()
    {
        float accel = grounded ? GroundAcceleration : AirAcceleration;

        // allow player to move when they are not already moving faster than the max speed
        if (RB.velocityX < MaxHorizontalSpeed && horizontalInput > 0f)
        {
            RB.AddForce(accel * Vector2.right, ForceMode2D.Impulse);
        }
        else if (RB.velocityX > -MaxHorizontalSpeed && horizontalInput < 0f)
        {
            RB.AddForce(accel * Vector2.left, ForceMode2D.Impulse);
        }

        // decelerate horizontally when no input in the same direction is registered
        // TODO: potentially make this an impulse based system, rather than directly modifying velocity?
        // not sure if that matters but something to consider
        if (RB.velocityX > 0f && horizontalInput == 0f)
        {
            RB.velocityX -= Math.Min(RB.velocity.x, accel * Time.deltaTime * DecelerationFactor);
        }
        else if (RB.velocityX < 0f && horizontalInput == 0f)
        {
            RB.velocityX -= -1 * Math.Min(-RB.velocityX, accel * Time.deltaTime * DecelerationFactor);
        }

        // rotate sprite when moving horizontally
        float angle = -1f * RB.velocityX * 1f;
        SRObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // called when moving input is registered
    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    // called when jump input is registered
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && RB.velocityY < JumpForce)
        {
            if (IsGrounded())
            {
                RB.velocityY = JumpForce;
            }
            else if (currDoubleJumps > 0)
            {
                RB.velocityY = JumpForce;
                currDoubleJumps--;
            }
        }

        if (context.canceled && RB.velocityY > 0f)
        {
            RB.velocityY *= 0.5f;
        }
    }

    // debugging function, shows jump boxcast
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube
        (
            transform.position - new Vector3(0f, BoxCollider.size.y * 0.5f - groundedBoxHeight * 0.25f),
            new Vector2(BoxCollider.size.x * 0.65f, groundedBoxHeight * 0.5f)
        );
    }
}
