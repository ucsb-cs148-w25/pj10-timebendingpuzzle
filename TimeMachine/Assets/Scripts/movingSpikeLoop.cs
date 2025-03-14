using System.Collections;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float speed = 3f;            // Speed of movement
    public float respawnRightX = 10f;   // X position on the right side to wrap to when moving left
    
    private Vector2 startPosition;      // Stores initial position

    void Start()
    {
        // Save the starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // If Q is held, move left; otherwise, move right
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Only respond if the collider has the Ground tag
        if (col.CompareTag("Ground"))
        {
            if (Input.GetKey(KeyCode.Q))
            {
                // We are moving left and just hit the left wall:
                // move the spike to the right side (wrap around)
                transform.position = new Vector2(8.19f, transform.position.y);
            }
            else
            {
                // Default behavior (e.g., reset to start position if moving right)
                // If you have a right wall, this is where you decide what happens
                transform.position = startPosition;
            }
        }
    }
}