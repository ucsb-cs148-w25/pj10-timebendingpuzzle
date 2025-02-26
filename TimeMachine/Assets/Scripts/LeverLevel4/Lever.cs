using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IRewindable
{
    public KeyCode interactionKey = KeyCode.E; // Key to interact with the bridge
    private bool isPlayerInRange = false; // Tracks if the player is near the lever
    private bool isBridgeDown = false; // Tracks bridge state

    private Transform bridge; // Reference to the bridge GameObject
    private Quaternion initialRotation; // Stores initial upright rotation
    private Quaternion downRotation; // Stores the 90-degree down rotation

    // State history for rewinding
    private LinkedList<bool> stateHistory = new LinkedList<bool>();

    void Start()
    {
        // Find the bridge GameObject by tag
        bridge = GameObject.FindGameObjectWithTag("Bridge").transform;

        if (bridge == null)
        {
            Debug.LogError("Bridge GameObject not found! Make sure it's tagged correctly.");
            return;
        }

        // Store the initial rotation of the bridge
        initialRotation = bridge.rotation;
        downRotation = initialRotation * Quaternion.Euler(0f, 0f, -90f); // 90-degree drop
    }

    void Update()
    {
        // Debugging: Check key press
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("E key pressed.");
        }

        // Toggle bridge position when player is in range and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Bridge toggled!");
            isBridgeDown = !isBridgeDown; // Toggle state
            bridge.rotation = isBridgeDown ? downRotation : initialRotation; // Instantly switch rotation
        }
    }

    // Detect player entering the trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered bridge lever range.");
            isPlayerInRange = true;
        }
    }

    // Detect player leaving the trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left bridge lever range.");
            isPlayerInRange = false;
        }
    }

    public void SaveState()
    {
        stateHistory.AddLast(isBridgeDown);
    }

    public void RewindState()
    {
        if (stateHistory.Count > 0)
        {
            isBridgeDown = stateHistory.Last.Value;
            bridge.rotation = isBridgeDown ? downRotation : initialRotation;
            stateHistory.RemoveLast();
        }
    }
}
