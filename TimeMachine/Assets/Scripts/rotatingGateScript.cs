using System.Collections;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.forward; // Rotation axis
    public float rotationSpeed = 90f; // Degrees per second
    private bool isRotating = false;
    private bool rotateForward = true; // Tracks the last state
    private bool activated = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) && !isRotating) // If Q is held, go back
        {
            if (activated == true){
                StartCoroutine(RotatePlatformCoroutine(rotateForward ? 90f : -90f)); 
                rotateForward = !rotateForward; // Toggle back
            }
            
        }
    }

    public void RotatePlatform() // Call this from a game button
    {
        activated = true;
        if (!isRotating)
        {
            float angle = rotateForward ? 90f : -90f;
            rotateForward = !rotateForward;
            StartCoroutine(RotatePlatformCoroutine(angle));
        }
    }

    private IEnumerator RotatePlatformCoroutine(float angle)
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(rotationAxis * angle);
        float elapsed = 0f;

        while (elapsed < (Mathf.Abs(angle) / rotationSpeed))
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / (Mathf.Abs(angle) / rotationSpeed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure exact rotation
        isRotating = false;
    }
}

