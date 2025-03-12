using UnityEngine;

public class GameButton : MonoBehaviour
{
    public RotatingPlatform platform; // Assign the platform in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player activates the button
        {
            platform.RotatePlatform();
        }
    }
}
