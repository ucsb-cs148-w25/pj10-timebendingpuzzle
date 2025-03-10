using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject movingPlatform; // Assign the platform in the Inspector
    public float sinkAmount = 0.1f; // Amount the button sinks
    public float sinkSpeed = 5f; // Speed of sinking animation
    public float platformSpeed = 2f; // Speed of platform movement
    public float moveDistance = 10f; // Amount to move left
    private Vector3 originalPosition; // Store the original position of the button
    private Vector3 targetPosition; // Target position for platform movement
    private bool isSinking = false; // Track whether the button is currently pressed
    private bool isMovingPlatform = false; // Track platform movement

    private void Start()
    {
        originalPosition = transform.position; // Save the button's initial position
        if (movingPlatform != null)
        {
            targetPosition = movingPlatform.transform.position - new Vector3(moveDistance, 0, 0); // Move left
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player stepped on the button. Moving platform left by 10 units!");
            isSinking = true;
            isMovingPlatform = true; // Start moving the platform
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player stepped off the button.");
            isSinking = false;
        }
    }

    private void Update()
    {
        // Handle the button sinking and returning to its original position
        if (isSinking)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition - new Vector3(0, sinkAmount, 0), Time.deltaTime * sinkSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * sinkSpeed);
        }

        // Move the platform left by 10 units when the player presses the button
        if (isMovingPlatform && movingPlatform != null)
        {
            movingPlatform.transform.position = Vector3.MoveTowards(
                movingPlatform.transform.position, 
                targetPosition, 
                platformSpeed * Time.deltaTime
            );

            // Stop moving when it reaches the target
            if (movingPlatform.transform.position == targetPosition)
            {
                isMovingPlatform = false;
            }
        }
    }
}
