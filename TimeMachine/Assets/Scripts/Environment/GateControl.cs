using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateControl : MonoBehaviour
{
    public Transform gate; // Assign the platform GameObject here
    public KeyCode interactionKey = KeyCode.E; // Key to interact with the lever

    private bool isPlayerInRange = false; // Tracks if the player is near the lever 


    void Update()
    {
        // Check for interaction when the player is in range
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {   
            gate.transform.position = new Vector3(37, -10, 0);
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
}



