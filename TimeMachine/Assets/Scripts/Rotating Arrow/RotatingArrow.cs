using UnityEngine;

public class RotatingArrow : MonoBehaviour
{
    public float rotationSpeed = 5000f; // Extreme speed
    private GameObject player;
    private Renderer playerRenderer;
    private Color originalColor;
    private Color blinkColor = new Color(156f / 255f, 55f / 255f, 84f / 255f, 1f); // #9C3754
    private Color blinkColor2 = new Color(37f / 255f, 150f / 255f, 190f / 255f, 1f); // #9C3754

    public float blinkSpeed = 20f; // Adjust speed of blinking

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerRenderer = player.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                originalColor = playerRenderer.material.color;
            }
        }
        else
        {
            Debug.LogWarning("Player with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate the arrow instantly
            transform.Rotate(0, 0, rotationSpeed);

            // Oscillate player's color smoothly
            if (playerRenderer != null)
            {
                float colorOsc = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
                playerRenderer.material.color = Color.Lerp(originalColor, blinkColor, colorOsc);
            }
        }
        else if (Input.GetKey(KeyCode.R))
        {
            // Oscillate player's color smoothly
            if (playerRenderer != null)
            {
                float colorOsc = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
                playerRenderer.material.color = Color.Lerp(Color.blue, Color.white, colorOsc);
            }
        }
        else
        {
            // Restore the player's original color when Q is released
            if (playerRenderer != null)
            {
                playerRenderer.material.color = originalColor;
            }
        }
    }
}
