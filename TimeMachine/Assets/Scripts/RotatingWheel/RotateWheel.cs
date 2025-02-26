using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheel : MonoBehaviour, IRewindable
{
    private Quaternion targetRotation;
    private bool isRotating = false;

    // Rotation speed
    public float rotationSpeed = 5f;
    
    // Rewind history
    private LinkedList<Quaternion> rotationHistory = new LinkedList<Quaternion>();

    private void Start()
    {
        targetRotation = transform.rotation; // Store initial rotation
    }

    public void Rotate()
    {
        if (!isRotating)
        {
            // Ensure only the X-axis is rotated by 360 degrees
            targetRotation *= Quaternion.Euler(180f, 0f, 0f);
            StartCoroutine(RotateToTarget());
        }
    }

    private IEnumerator RotateToTarget()
    {
        isRotating = true;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            yield return null;
        }
        transform.rotation = targetRotation; // Snap to exact rotation
        isRotating = false;
    }

    public void SaveState()
    {
        rotationHistory.AddLast(transform.rotation);
    }

    public void RewindState()
    {
        if (rotationHistory.Count > 0)
        {
            transform.rotation = rotationHistory.Last.Value;
            targetRotation = transform.rotation;
            rotationHistory.RemoveLast();
        }
    }
}
