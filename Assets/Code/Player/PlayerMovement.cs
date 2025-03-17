
using System;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    // References 
    public BoxCollider2D BoxCollider;
    public Rigidbody2D RB;
    public GameObject SRObject;
    public LayerMask GroundLayer;
    public ClassBase playerClass;
    public DemoClass attackScript;

    // Movement
    public float MaxHorizontalSpeed = 8f;
    public float maxFallingSpeed = 12f;
    public float maxFastFallingSpeed = 32;
    public float deadZone = 25f;
    public float JumpForce = 18f;
    public float bufferedJumpLifeTime = 0.2f;
    public float dashForce = 20;
    public bool dashTest = false;
    public float bufferedDashLifeTime = 0.3f;
    public int dropThroughHoldTime = 10;
    public float dropAheadSpeedModifier = 0.05f;
    public float coyoteTimeThreshold = 0.5f;
    // Accelerations
    public float AirAcceleration = 3f;
    public float GroundAcceleration = 4f;
    public float DecelerationFactor = 24f;
    // Limiters
    public float DashCooldown = 1;
    public int MaxDoubleJumps = 1;

    public bool facingRight;

    // Movement
    Vector2 currentMovement;
    Vector2 lastMovement = new Vector2(0, 0);
    float horizontalInput;
    bool speedFalling = false;
    bool canDash = true;
    float dashCooldownCount = 0;
    bool dashTestLast = false;
    bool dashQueued = false;
    float queuedDashLife = 0;
    int currDoubleJumps;
    bool jumpedLast = false;
    bool jumpQueued = false;
    float queuedJumpLife = 0;
    // Grounding
    float groundedBoxHeight = 0.5f;
    bool grounded;
    bool groundedLast;
    bool coyoteTime = false;
    float coyoteTimeCountdown = 0;
    // Drop Through
    bool dropThroughTriger = false;
    bool canDropThrough = false;
    int dropThroughCounter = 0;
    float dropAheadOverhead = 0.3f;
    // Attack
    bool isAttack = false;
    float afterAttackWait = 0.8f;
    float attackTimer = 0;

    void Start()
    {
        currDoubleJumps = MaxDoubleJumps;

        //Get Refrence to the Class
        playerClass = GetComponent<ClassBase>();
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
        if (grounded)
        {
            if (!InPlatform())
            {
                currDoubleJumps = MaxDoubleJumps;
            }
        }

        // jump buffering life time timer
        if (jumpQueued)
        {
            queuedJumpLife -= Time.deltaTime;
            if (queuedJumpLife <= 0)
            {
                jumpQueued = false;
                queuedJumpLife = 0;
            }
        }

        // dev dash test
        if (dashTest != dashTestLast)
        {
            DoDash(new Vector2(1, 0));
            dashTestLast = dashTest;
        }

        // dash buffering
        if (dashQueued)
        {
            queuedDashLife -= Time.deltaTime;
            if (queuedDashLife <= 0)
            {
                dashQueued = false;
                queuedDashLife = 0;
            }
        }

        // dash cooldown
        if (!canDash)
        {
            dashCooldownCount -= Time.deltaTime;
            if (dashCooldownCount <= 0)
            {

                if (dashQueued)
                {
                    dashQueued = false;
                    queuedDashLife = 0;
                    DoDash(currentMovement);
                }
                else
                {
                    dashCooldownCount = DashCooldown;
                    canDash = true;
                }
            }
        }

        // coyote time countdown timer
        if (coyoteTime)
        {
            coyoteTimeCountdown -= Time.deltaTime;
            if (coyoteTimeCountdown <= 0)
            {
                coyoteTime = false;
                coyoteTimeCountdown = 0;
            }
        }

        // adding a delay from fall-through and down attacks
        if (isAttack && attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttack = false;
                attackTimer = 0;
            }
        }
    }

    // is called at same rate as Unity physics so rigidbody physics should be done here
    void FixedUpdate()
    {
        grounded = IsGrounded();

        float accel = grounded ? GroundAcceleration : AirAcceleration;
        // allow player to move when they are not already moving faster than the max speed
        if (RB.velocityX < MaxHorizontalSpeed * horizontalInput && horizontalInput > 0f)
        {
            if (RB.velocityX + (accel * horizontalInput) < MaxHorizontalSpeed)
            {
                RB.AddForce(new Vector2(accel * horizontalInput, 0), ForceMode2D.Impulse);
            }
            else
            {
                RB.velocityX = MaxHorizontalSpeed;
            }
        }
        else if (RB.velocityX > -MaxHorizontalSpeed * -horizontalInput && horizontalInput < 0f)
        {
            if (RB.velocityX + (accel * horizontalInput) > -MaxHorizontalSpeed)
            {
                RB.AddForce(new Vector2(accel * horizontalInput, 0), ForceMode2D.Impulse);
            }
            else
            {
                RB.velocityX = -MaxHorizontalSpeed;

            }
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

        /* // rotate sprite when moving horizontally
        float angle = -1f * RB.velocityX * 1f;
        SRObject.transform.rotation = Quaternion.Euler(0f, 0f, angle); */

        // fast falling and max falling speed
        if (speedFalling)
        {
            if (RB.velocityY > -maxFastFallingSpeed)
            {
                RB.AddForce(new Vector2(0, -maxFastFallingSpeed), ForceMode2D.Force);
            }
            else
            {
                RB.velocityY = -maxFastFallingSpeed;
            }
        }
        else if (RB.velocityY < -maxFallingSpeed)
        {
            RB.velocityY = -maxFallingSpeed;
        }

        // jumping when on ground with a queued jump
        if (jumpQueued && grounded)
        {
            AudioManager.PlaySound("Jump1");
            RB.velocityY = JumpForce;
            jumpedLast = true;
            jumpQueued = false;
            queuedJumpLife = 0;
        }

        // coyote jump countdown start
        if (!grounded && grounded != groundedLast && !jumpedLast)
        {
            coyoteTime = true;
            coyoteTimeCountdown = coyoteTimeThreshold;
        }

        // resetting the last jump when there can't have been a jump last frame
        if (jumpedLast && !grounded)
        {
            jumpedLast = false;
        }

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
        if (canDropThrough && BoxCollider.enabled && dropThroughTriger && !isAttack && dropThroughCounter >= dropThroughHoldTime)
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

        // recording the last grounded
        groundedLast = grounded;
        
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
        AudioManager.PlaySound("Dash1");
        RB.AddForce(new Vector2(dir.x * dashForce, dir.y * dashForce), ForceMode2D.Impulse);
        canDash = false;
    }

    // called when moving input is registered
    public void Move(InputAction.CallbackContext context)
    {
        float decDeadZone = deadZone * (1f / 100f);
        currentMovement = context.ReadValue<Vector2>();

        if (currentMovement.x != 0) { facingRight = currentMovement.x > 0; }

        if (currentMovement.x >= -decDeadZone && currentMovement.x <= decDeadZone)
        {
            currentMovement.x = 0;
        }
        if (currentMovement.y >= -decDeadZone && currentMovement.y <= decDeadZone)
        {
            currentMovement.y = 0;
        }

        if (Math.Abs(lastMovement.x) < Math.Abs(currentMovement.x) || Math.Abs(lastMovement.y) < Math.Abs(currentMovement.y))
        {
            lastMovement = currentMovement;
        }

        horizontalInput = currentMovement.x;

        // checking for "down" being pressed
        dropThroughTriger = false;
        speedFalling = false;

        if (currentMovement.y < -decDeadZone)
        {
            speedFalling = true;
            dropThroughTriger = true;
        }
    }

    // called when dash input is registered
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && currentMovement != Vector2.zero)
        {
            if (canDash)
            {
                DoDash(currentMovement);
            }
            else
            {
                // queuing a dash
                dashQueued = true;
                queuedDashLife = bufferedDashLifeTime;
            }
        }
    }

    public void MeleeAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttack = true;

            if (Math.Abs(lastMovement.x) >= Math.Abs(lastMovement.y))
            {
                if (lastMovement.x >= 0)
                {
                    attackScript.DemoSide(false, false);
                }
                else if (lastMovement.x < 0)
                {
                    attackScript.DemoSide(true, false);
                }
            }
            else
            {
                if (lastMovement.y >= 0)
                {
                    attackScript.DemoUp(false);
                }
                else if (lastMovement.y < 0)
                {
                    attackScript.DemoDown(false);
                }
            }
        }
        else if (context.canceled)
        {
            attackTimer = afterAttackWait;
        }
    }

    public void RangedAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttack = true;

            if (Math.Abs(lastMovement.x) >= Math.Abs(lastMovement.y))
            {
                if (lastMovement.x >= 0)
                {
                    attackScript.DemoSide(false, true);
                }
                else if (lastMovement.x < 0)
                {
                    attackScript.DemoSide(true, true);
                }
            }
            else
            {
                if (lastMovement.y >= 0)
                {
                    attackScript.DemoUp(true);
                }
                else if (lastMovement.y < 0)
                {
                    attackScript.DemoDown(true);
                }
            }
        }
        else if (context.canceled)
        {
            attackTimer = afterAttackWait;
        }
    }

    // called when jump input is registered
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && RB.velocityY < JumpForce)
        {
            if (IsGrounded() /* && !InPlatform() */)
            {
                // jump
                AudioManager.PlaySound("Jump1");
                RB.velocityY = JumpForce;
                jumpedLast = true;

            }
            else if (currDoubleJumps > 0)
            {
                AudioManager.PlaySound("Jump1");
                RB.velocityY = JumpForce;
                if (coyoteTime)
                {
                    // coyote jump
                    coyoteTime = false;
                    coyoteTimeCountdown = 0;
                }
                else
                {
                    // double jump
                    currDoubleJumps--;
                }
            }
            else if (!InPlatform())
            {
                // queuing a jump
                jumpQueued = true;
                queuedJumpLife = bufferedJumpLifeTime;
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
        Gizmos.color = Color.red;
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
