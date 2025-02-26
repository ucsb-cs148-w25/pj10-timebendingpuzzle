using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera targetCamera; // Assign this in the Inspector
    public int newPriority = 20; // Higher priority means this camera takes over
    private int originalPriority; // Store the original priority

    private void Start()
    {
        if (targetCamera != null)
        {
            originalPriority = targetCamera.Priority; // Store the initial priority
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && targetCamera != null)
        {
            targetCamera.Priority = newPriority; // Increase priority when player enters
        }
    }

}
