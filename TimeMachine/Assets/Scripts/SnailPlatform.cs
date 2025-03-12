using UnityEngine;

public class SnailPlatform : MonoBehaviour
{
    public Vector3 respawnPosition; // Set this in the Inspector (Player's start position)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if Player lands on platform
        {
            AttachAndDetach playerScript = collision.gameObject.GetComponent<AttachAndDetach>();

            if (playerScript != null && !playerScript.IsAttachedToSnail()) // If not attached, respawn
            {
                collision.gameObject.transform.position = respawnPosition;
                Debug.Log("Player fell onto SnailPlatform without snail! Respawning...");
            }
        }
    }
}
