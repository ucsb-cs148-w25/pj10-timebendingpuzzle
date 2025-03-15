using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeControl : MonoBehaviour
{
    public Transform bridge; // Assign the platform GameObject here
    public float rotationSpeed;
    public KeyCode interactionKey = KeyCode.E; // Key to interact with the lever

    private bool isPlayerInRange = false; // Tracks if the player is near the lever
    private bool rotating;
    private float rotatedAngle;
    private float rotationStep;

    void Start(){
        rotatedAngle = 0f;
    }


    void Update()
    {
        rotationStep = rotationSpeed * Time.deltaTime;

        // Debug: Confirm key press is detected
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("E key pressed.");
        }

        // Check for interaction when the player is in range
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {   
            Rotate();
        }
    }

    // Trigger methods for 2D physics
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered lever range.");
            isPlayerInRange = true; // Set the flag when the player enters
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left lever range.");
            isPlayerInRange = false; // Reset the flag when the player exits
        }
    }

    void Rotate(){
        while (rotatedAngle + rotationStep <= 90f){
            bridge.transform.Rotate(0, 0, -rotationStep);
            rotatedAngle += rotationStep;
        } // Rotates instantly / fast as fuck. Whatever I'm over it
    }
}



