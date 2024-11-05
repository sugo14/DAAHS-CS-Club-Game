using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    public Rigidbody2D player1;
    public int maxpresscount = 2;
    public float timecount = 2f;
    private int orpressconut = 0;
    private float timenow = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D player1 = GetComponent<Rigidbody2D>();
        player1.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            if (Time.time - timenow > timecount)
            {
                orpressconut = 0;
                timenow = Time.time;
            }
            if (orpressconut < maxpresscount)
            {
                orpressconut++;
                player1.linearVelocity = Vector2.up * 7;
            }

        }
        if (Input.GetKey(KeyCode.D) == true)
        {
            player1.linearVelocityX = 5;
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            player1.linearVelocityX = -5;
        }
        if (Input.GetKey(KeyCode.S) == true)
        {
            if (player1.position.y > 0)
            {
                player1.linearVelocityY = player1.linearVelocityY - 0.1f;
            }

        }
        if (Input.GetKey(KeyCode.D) == true)
        {
            if (Input.GetKey(KeyCode.A) == true)
            {
                player1.linearVelocityX = 0;
            }
        }
        if (player1.position.y <= -10)
        {
            Vector2 newpos = transform.position;
            newpos.y = 10;
            newpos.x = Random.Range(-10, 7);
            transform.position = newpos;
            Vector2 newvelocity = player1.linearVelocity;
            newvelocity.y = 0;
            player1.linearVelocity = newvelocity;



        }
    }
}