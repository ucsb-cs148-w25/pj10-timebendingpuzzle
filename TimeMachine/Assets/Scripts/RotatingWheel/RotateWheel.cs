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
    private LinkedList<Tuple<bool, Quaternion>> rotationHistory = new LinkedList<Tuple<bool,Quaternion>>();

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
        rotationHistory.AddLast(Tuple.Create(isRotating, targetRotation));
    }

    public void RewindState()
    {
        if(rotationHistory.Last.Value.Item1 == false){ // if in  the most recent saved timepoint, the door WASNT rotating
            targetRotation = rotationHistory.Last.Value.Item2;
        } else if(!isRotating){ // if door isnt currently rotating, but the last saved timepoint door was rotating
            StartCoroutine(RotateToTarget());
        }
        rotationHistory.RemoveLast();
    }

    public void ClearHistory()
    {
        rotationHistory.Clear();
    }
}