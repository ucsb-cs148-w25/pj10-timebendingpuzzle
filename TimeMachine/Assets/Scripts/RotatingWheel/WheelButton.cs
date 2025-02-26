using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelButton : MonoBehaviour
{
    public GameObject rotatingWheel; // Assign the rotating wheel in the Inspector
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

            RotateWheel wheelScript = rotatingWheel.GetComponent<RotateWheel>();
            if (wheelScript != null)
            {
                Debug.Log("Wheel script found! Rotating the wheel...");
                wheelScript.Rotate();
            }
            else
            {
                Debug.LogWarning("RotateWheel script not found on the assigned rotating wheel object!");
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
