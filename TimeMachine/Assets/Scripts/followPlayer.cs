using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;  // Assign your player character here
    public float followSpeed = 5f;  // Adjust speed for smooth movement
    private float fixedYPosition;  // Will store the camera’s starting Y

    void Start()
    {
        // Capture the initial Y position of the camera
        fixedYPosition = transform.position.y;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target’s X position, keeping the initial Y
            Vector3 newPosition = new Vector3(target.position.x, fixedYPosition, transform.position.z);

            // Smoothly follow using SmoothDamp (better than Lerp)
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }
    }
}
