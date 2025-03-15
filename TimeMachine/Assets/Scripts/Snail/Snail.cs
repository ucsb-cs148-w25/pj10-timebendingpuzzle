using UnityEngine;
using System.Collections.Generic;

public class Snail : MonoBehaviour, IRewindable
{
    public Sprite noBodySprite; // Assign in Unity (shell-only sprite)
    private SpriteRenderer spriteRenderer;
    public bool hasBody = true; // Snail starts with its body
    public Sprite fullBodySprite;
    
    private LinkedList<bool> stateHistory = new LinkedList<bool>();
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // If it hits a platform, lose body
        {
            LoseBody();
        }
    }

    void LoseBody()
    {
        if (!hasBody) return; // If already dead, do nothing

        hasBody = false;
        spriteRenderer.sprite = noBodySprite;

        // ✅ Stop all movement when snail dies
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("Snail lost its body! Cannot be ridden.");
    }

    public void SaveState()
    {
        stateHistory.AddLast(hasBody);
    }

    public void RewindState()
    {
        if (stateHistory.Count > 0)
        {
            hasBody = stateHistory.Last.Value;
            spriteRenderer.sprite = hasBody ? fullBodySprite : noBodySprite;
            stateHistory.RemoveLast();

            // ✅ Re-enable movement if body is restored
            if (hasBody)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    public void ClearHistory()
    {
        stateHistory.Clear();
    }
}
