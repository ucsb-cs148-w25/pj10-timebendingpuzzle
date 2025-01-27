using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public GameObject rotatingDoor; // Assign the rotating door in the Inspector
    public float sinkAmount = 0.1f; // Amount the button sinks
    public float sinkSpeed = 5f; // Speed of sinking animation
    private Vector3 originalPosition; // Store the original position of the button
    private bool isSinking = false; // Track whether the button is currently pressed

    private void Start()
    {
        originalPosition = transform.position; // Save the button's initial position
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player stepped on the button.");
            isSinking = true;

            RotatingDoor doorScript = rotatingDoor.GetComponent<RotatingDoor>();
            if (doorScript != null)
            {
                Debug.Log("Door script found! Rotating the door...");
                doorScript.RotateDoor();
            }
            else
            {
                Debug.LogWarning("RotatingDoor script not found on the assigned rotating door object!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player stepped off the button.");
            isSinking = false; // Reset the sinking state
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
    }
}
