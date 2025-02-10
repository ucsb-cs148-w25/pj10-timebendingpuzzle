using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour, IRewindable
{
    public Transform platform; // Assign the platform GameObject here
    public Vector3 platformUpPosition; // Position where the platform moves up
    public Vector3 platformDownPosition; // Position where the platform moves down
    public float platformMoveSpeed = 2f; // Speed of platform movement
    public KeyCode interactionKey = KeyCode.E; // Key to interact with the lever

    private bool isPlatformUp = true; // Tracks the current state of the platform
    private bool isPlayerInRange = false; // Tracks if the player is near the lever

    //state history
    private LinkedList<bool> stateHistory = new LinkedList<bool>();

    void Start()
    {
        // Ensure the platform starts at the correct position
        platform.position = isPlatformUp ? platformUpPosition : platformDownPosition;
    }

    void Update()
    {
        // Debug: Confirm key press is detected
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("E key pressed.");
        }

        // Check for interaction when the player is in range
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Lever activated!");
            isPlatformUp = !isPlatformUp; // Toggle platform state
        }

        // Smoothly move the platform to the target position
        Vector3 targetPosition = isPlatformUp ? platformUpPosition : platformDownPosition;
        platform.position = Vector3.MoveTowards(platform.position, targetPosition, platformMoveSpeed * Time.deltaTime);
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

    public void SaveState(){
        stateHistory.AddLast(isPlatformUp);
    }

    public void RewindState(){
        isPlatformUp = stateHistory.Last.Value;
        stateHistory.RemoveLast();
    }
}



