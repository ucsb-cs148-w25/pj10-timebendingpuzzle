using UnityEngine;

public class Snail : MonoBehaviour
{
    public Sprite noBodySprite; // Assign this in Unity (this is the "shell-only" sprite)
    private SpriteRenderer spriteRenderer;
    public bool hasBody = true; // Snail starts with its body

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // Ensure spikes have the correct tag
        {
            LoseBody();
        }
    }

    void LoseBody()
    {
        if (hasBody) // Only remove body if it hasn't been removed yet
        {
            hasBody = false; // Snail no longer has a body
            spriteRenderer.sprite = noBodySprite; // Change sprite to show only the shell
            Debug.Log("Snail lost its body! Cannot be ridden.");
        }
    }
}
