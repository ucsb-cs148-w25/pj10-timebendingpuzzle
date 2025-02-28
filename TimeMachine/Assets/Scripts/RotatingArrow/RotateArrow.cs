using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    public float rotationSpeed = 5000f; // Increase for extreme speed

    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) // Check if Q is pressed
        {
            transform.Rotate(0, 0, rotationSpeed); // No Time.deltaTime, rotates instantly
        }
    }
}
