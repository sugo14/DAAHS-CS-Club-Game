
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Rigidbody2D rb;
    public GameObject srObject;
    public LayerMask groundLayer;

    public float groundedBoxHeight = 0.5f;
    public float maxHorizontalSpeed = 8f;
    public float jumpForce = 18f;
    public float airAccel = 100f;
    public float groundAccel = 160f;
    public int maxDoubleJumps = 3;

    int currDoubleJumps;
    float horizontalInput;
    bool grounded;

    void Start() {
        currDoubleJumps = maxDoubleJumps;
    }

    // debugging function for collisions
    void Db(Collider2D collider) {
        Debug.Log("Collided with: " + collider.gameObject.name);
        Debug.Log("Collider Type: " + collider.GetType());
        Debug.Log("Is Trigger: " + collider.isTrigger);
        Debug.Log("Bounds: " + collider.bounds);
        Debug.Log("Offset: " + collider.offset);
        Debug.Log("Size: " + collider.bounds.size);
    }

    // check if the player is on the ground using a boxcast
    // from the player's feet
    bool IsGrounded() {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position - new Vector3(0f, boxCollider.size.y * 0.5f - groundedBoxHeight * 0.25f),
            new Vector2(boxCollider.size.x * 0.65f, groundedBoxHeight * 0.5f),
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return hit.collider != null;
    }

    void Update() {
        grounded = IsGrounded();
        if (grounded) { currDoubleJumps = maxDoubleJumps; }
    }

    // is called at same rate as Unity physics
    // so rigidbody physics should be done here
    void FixedUpdate() {
        float accel = grounded ? groundAccel : airAccel;

        // accelerate horizontally if input is registered
        rb.linearVelocityX += horizontalInput * accel * Time.deltaTime;

        // decelerate horizontally when no input is registered
        if (horizontalInput == 0f) {
            if (rb.linearVelocityX > 0f) {
                rb.linearVelocityX -= Math.Min(rb.linearVelocityX, accel * Time.deltaTime);
            }
            else if (rb.linearVelocityX < 0f) {
                rb.linearVelocityX -= -1 * Math.Min(-rb.linearVelocityX, accel * Time.deltaTime);
            }
        }

        // enforce maximum horizontal speed of player
        if (rb.linearVelocityX > maxHorizontalSpeed) {
            rb.linearVelocityX = maxHorizontalSpeed;
        }
        else if (rb.linearVelocityX < -maxHorizontalSpeed) {
            rb.linearVelocityX = -maxHorizontalSpeed;
        }

        // rotate sprite when moving horizontally
        float angle = -1f * rb.linearVelocityX * 1f;
        srObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // called when moving input is registered
    public void Move(InputAction.CallbackContext context) {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    // called when jump input is registered
    public void Jump(InputAction.CallbackContext context) {
        if (context.performed && rb.linearVelocityY < jumpForce) {
            if (IsGrounded()) {
                rb.linearVelocityY = jumpForce;
            }
            else if (currDoubleJumps > 0) {
                rb.linearVelocityY = jumpForce;
                currDoubleJumps--;
            }
        }

        if (context.canceled && rb.linearVelocityY > 0f) {
            rb.linearVelocityY *= 0.5f;
        }
    }

    // debugging function, shows jump boxcast
    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position - new Vector3(0f, boxCollider.size.y * 0.5f - groundedBoxHeight * 0.25f), new Vector2(boxCollider.size.x * 0.65f, groundedBoxHeight * 0.5f));
    }
}
