using UnityEngine;

public class AttachAndDetach : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float jumpForce = 8f; // Jump force when detaching
    private bool isAttached = false;
    private Transform attachedSnail;
    private Rigidbody2D rb;
    private Rigidbody2D snailRb;
    private SpriteRenderer playerRenderer;
    private SpriteRenderer snailRenderer;
    private Vector3 offset; // Offset to position player correctly on snail

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (isAttached && attachedSnail != null)
        {
            // Move the snail and player together
            snailRb.velocity = new Vector2(moveInput * moveSpeed, snailRb.velocity.y);
            transform.position = attachedSnail.position + offset; // Keep player on top

            // Flip both snail and player based on movement direction
            if (moveInput > 0)
            {
                playerRenderer.flipX = false;
                snailRenderer.flipX = true;
            }
            else if (moveInput < 0)
            {
                playerRenderer.flipX = true;
                snailRenderer.flipX = false;
            }
        }

        // Press Space to detach
        if (isAttached && Input.GetKeyDown(KeyCode.Space))
        {
            DetachFromSnail();
        }
    }

    private void FixedUpdate()
    {
        if (isAttached && attachedSnail != null)
        {
            // Ensure the snail (and player) fall naturally with gravity
            rb.velocity = snailRb.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Snail")) // Make sure the Snail has the "Snail" tag
        {
            Snail snailScript = collision.gameObject.GetComponent<Snail>();

            // Only attach if the snail has its body
            if (snailScript != null && snailScript.hasBody)
            {
                // Check if the player lands on top of the snail
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (Vector2.Dot(contact.normal, Vector2.up) > 0.7f) // Check if collision is mostly from above
                    {
                        AttachToSnail(collision.transform);
                        return; // Stop checking once we confirm top collision
                    }
                }
            }
            else
            {
                Debug.Log("Cannot attach! Snail has lost its body.");
            }
        }
    }

    void AttachToSnail(Transform snail)
    {
        isAttached = true;
        attachedSnail = snail;
        snailRb = snail.GetComponent<Rigidbody2D>();
        snailRenderer = snail.GetComponent<SpriteRenderer>();

        // Snap player to the correct position on top of the snail
        offset = new Vector3(0, snail.GetComponent<Collider2D>().bounds.extents.y + GetComponent<Collider2D>().bounds.extents.y, 0);
        transform.position = snail.position + offset;

        transform.SetParent(snail);
        rb.velocity = snailRb.velocity; // Sync velocity with the snail
        rb.isKinematic = false; // Player should move with gravity

        Debug.Log("Attached to snail!");
    }

    void DetachFromSnail()
    {
        isAttached = false;
        transform.SetParent(null);
        rb.isKinematic = false; // Re-enable player physics
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Jump on detach
    }

    public bool IsAttachedToSnail()
    {
        return isAttached; // Returns true if attached to the snail
    }
}
