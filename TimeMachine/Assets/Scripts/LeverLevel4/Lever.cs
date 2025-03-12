using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IRewindable
{
    public KeyCode interactionKey = KeyCode.E; // Key to interact with the lever
    private bool isPlayerInRange = false; // Tracks if the player is near the lever
    private bool isBridgeMovingDown = false; // Tracks rotation state

    private Transform bridge; // Reference to the bridge GameObject
    private Quaternion initialRotation; // Stores initial upright rotation
    private Quaternion targetRotation; // Stores the 130-degree down rotation

    public float rotationSpeed = 100f; // Rotation speed in degrees per second

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
        targetRotation = initialRotation * Quaternion.Euler(0f, 0f, -130f); // Rotate 130 degrees down
    }

    void Update()
    {
        // Toggle rotation when player is in range and presses 'E'
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Bridge rotation toggled!");
            isBridgeMovingDown = !isBridgeMovingDown; // Toggle state
        }

        // Rotate smoothly using RotateTowards
        if (isBridgeMovingDown)
        {
            bridge.rotation = Quaternion.RotateTowards(bridge.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            bridge.rotation = Quaternion.RotateTowards(bridge.rotation, initialRotation, rotationSpeed * Time.deltaTime);
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
        stateHistory.AddLast(isBridgeMovingDown);
    }

    public void RewindState()
    {
        if (stateHistory.Count > 0)
        {
            isBridgeMovingDown = stateHistory.Last.Value;
            stateHistory.RemoveLast();
        }
    }

    public void ClearHistory()
    {
        stateHistory.Clear();
    }
}
