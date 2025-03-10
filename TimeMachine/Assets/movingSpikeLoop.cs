using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    public float speed = 3f; // Speed of movement
    private Vector2 startPosition; // Stores initial position

    void Start()
    {
        startPosition = transform.position; // Save the starting position
    }

    void Update()
    {
        // Move the spike to the right
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground")) // Ensure wall has the "Ground" tag
        {
            Respawn(); // Respawn when it hits the wall
        }
    }

    void Respawn()
    {
        transform.position = startPosition; // Reset to initial position
    }
}
