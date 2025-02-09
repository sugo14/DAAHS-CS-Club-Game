
using System;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.PlasticSCM.Editor.WebApi;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovementScript : MonoBehaviour
{
    // References 
    public BoxCollider2D BoxCollider;
    public Rigidbody2D RB;
    public GameObject SRObject;
    public LayerMask GroundLayer;

    // Movement
    public float MaxHorizontalSpeed = 8f;
    public float deadZone = 15f;
    public float JumpForce = 18f;
    public float DashForce = 20;
    public bool dashTest = false;
    public int dropThroughHoldTime = 20;
    public float dropAheadSpeedModifier = 0.05f; 
    // Accelerations
    public float AirAcceleration = 3f;
    public float GroundAcceleration = 4f;
    public float DecelerationFactor = 24f;
    // Limiters
    public float DashCooldown = 1;
    public int MaxDoubleJumps = 1;


    // Movement
    float horizontalInput;
    bool canDash = true;
    float dashCooldownCount = 0;
    bool dashTestLast = false;
    int currDoubleJumps;
    Vector2 currentMovement;
    // Grounding
    float groundedBoxHeight = 0.5f;
    bool grounded;
    bool dropThroughTriger = false;
    bool canDropThrough = false;
    int dropThroughCounter = 0;
    float dropAheadOverhead = 0.3f;


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

    // check if player is in a platform tag with "Passthrough"
    bool InPlatform()
    {
        RaycastHit2D hit = Physics2D.BoxCast
        (
           transform.position, 
           new Vector2(BoxCollider.size.x - 0.26f, BoxCollider.size.y - 0.01f),
           0f,
           Vector2.zero,
           0.1f,
           GroundLayer
        );

        if (hit.collider != null)
        {
            return hit.collider.gameObject.CompareTag("Passthrough");
        }

        return false;
    }

    // checking if a platform tag with "Passthrough" is under player 
    bool CanDropAhead()
    {
        RaycastHit2D hit = Physics2D.BoxCast
        (
           transform.position - new Vector3(0, (BoxCollider.size.y + (dropAheadOverhead + (-RB.velocityY * dropAheadSpeedModifier)) / 2) - 0.5f),
           new Vector2(BoxCollider.size.x - 0.25f, dropAheadOverhead + (-RB.velocityY * dropAheadSpeedModifier)),
           0f,
           Vector2.down,
           0.1f,
           GroundLayer
        );

        if (hit.collider != null)
        {
            return hit.collider.gameObject.CompareTag("Passthrough");
        }

        return false;
    }


    void Update()
    {
        grounded = IsGrounded();
        if (grounded)
        {
            if (!InPlatform())
            {
                currDoubleJumps = MaxDoubleJumps;
            }
        }
        
        // dev dash test
        if (dashTest != dashTestLast)
        {
            DoDash(new Vector2(1, 0));
            dashTestLast = dashTest;
        }

        // Dash cooldown
        if (!canDash)
        {
            dashCooldownCount -= Time.deltaTime;
        }
        if (dashCooldownCount <= 0)
        {
            dashCooldownCount = DashCooldown;
            canDash = true;
        }

    }

    // is called at same rate as Unity physics so rigidbody physics should be done here
    void FixedUpdate()
    {
        float accel = grounded ? GroundAcceleration : AirAcceleration;
        // allow player to move when they are not already moving faster than the max speed
        if (RB.velocityX < MaxHorizontalSpeed && horizontalInput > 0f)
        {
            RB.AddForce(new Vector2(accel * horizontalInput, 0), ForceMode2D.Impulse);
        }
        else if (RB.velocityX > -MaxHorizontalSpeed && horizontalInput < 0f)
        {
            RB.AddForce(new Vector2(accel * horizontalInput, 0), ForceMode2D.Impulse);
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

        // incrementing and resetting drop counter when down is held or released
        if (dropThroughTriger)
        {
            dropThroughCounter++;
        }
        else
        {
            dropThroughCounter = 0;
        }

        // allowing smooth dropping through platforms from air
        if (!canDropThrough && !grounded)
        {
            if (CanDropAhead())
            {
                canDropThrough = true;
            }
        }

        // Triggering drop through 
        if (canDropThrough && BoxCollider.enabled && dropThroughTriger && dropThroughCounter >= dropThroughHoldTime)
        {
            // Debug.Log("drop called");
            BoxCollider.enabled = false;
            dropThroughCounter = 0;
        }

        // enabling BoxCollider when outside of the platform that was dropped through 
        if (!BoxCollider.enabled)
        {
            if (!InPlatform() && !CanDropAhead())
            {
                BoxCollider.enabled = true;
                canDropThrough = false;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // checking if player is on a platform with  the "Passthrough" tag
        if (collision.gameObject.CompareTag("Passthrough"))
        {
            canDropThrough = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // checking if player has left a platform with  the "Passthrough" tag
        if (collision.gameObject.CompareTag("Passthrough"))
        {
            canDropThrough = false;
        }
    }

    // does a dash
    public void DoDash(Vector2 dir)
    {
        // Debug.Log("dash");
        RB.AddForce(new Vector2(dir.x * DashForce, dir.y * DashForce), ForceMode2D.Impulse);
        canDash = false;
    }

    // called when moving input is registered
    public void Move(InputAction.CallbackContext context)
    {
        float decDeadZone = deadZone * (1f / 100f);
        currentMovement = context.ReadValue<Vector2>();

        if (currentMovement.x >= -decDeadZone && currentMovement.x <= decDeadZone)
        {
            currentMovement.x = 0;
        }
        if (currentMovement.y >= -decDeadZone && currentMovement.y <= decDeadZone)
        {
            currentMovement.y = 0;
        }

        horizontalInput = currentMovement.x;

        // checking for "down" being pressed
        dropThroughTriger = false;
        if (currentMovement.y < -decDeadZone)
        {
            dropThroughTriger = true;
        }
    }


    // called when dash input is registered
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash && currentMovement != Vector2.zero)
        {
            DoDash(currentMovement);
        }
    }

    // called when jump input is registered
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && RB.velocityY < JumpForce)
        {
            if (IsGrounded() && !InPlatform())
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
        // IsGrounded box
        Gizmos.DrawWireCube
        (
            transform.position - new Vector3(0f, BoxCollider.size.y * 0.5f - groundedBoxHeight * 0.25f),
            new Vector2(BoxCollider.size.x * 0.65f, groundedBoxHeight * 0.5f)
        );
        // InPlatform box
        Gizmos.DrawWireCube
        (
            transform.position,
            new Vector2(BoxCollider.size.x - 0.26f, BoxCollider.size.y - 0.01f)
        );
        // CanDropAhead box
        Gizmos.DrawWireCube
        (
            transform.position - new Vector3(0, (BoxCollider.size.y + (dropAheadOverhead + (-RB.velocityY * dropAheadSpeedModifier)) / 2) - 0.5f),
            new Vector2(BoxCollider.size.x - 0.25f, dropAheadOverhead + (-RB.velocityY * dropAheadSpeedModifier))
        );
    }
}
