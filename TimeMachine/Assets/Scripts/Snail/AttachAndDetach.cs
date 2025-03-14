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
    private Snail snailScript;

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
            if (snailScript.hasBody) // Move only if the snail has a body
            {
                snailRb.velocity = new Vector2(moveInput * moveSpeed, snailRb.velocity.y);
            }

            transform.position = attachedSnail.position + offset; // Keep player on top

            // Flip both snail and player based on movement direction
            if (moveInput > 0)
            {
                playerRenderer.flipX = false;
                if (snailScript.hasBody) snailRenderer.flipX = true;
            }
            else if (moveInput < 0)
            {
                playerRenderer.flipX = true;
                if (snailScript.hasBody) snailRenderer.flipX = false;
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
            rb.velocity = snailRb.velocity; // Ensure the player moves naturally with the snail
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Snail"))
        {
            snailScript = collision.gameObject.GetComponent<Snail>();

            if (snailScript != null && snailScript.hasBody)
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    if (Vector2.Dot(contact.normal, Vector2.up) > 0.7f) // Ensure player lands from above
                    {
                        AttachToSnail(collision.transform);
                        return;
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

        offset = new Vector3(0, snail.GetComponent<Collider2D>().bounds.extents.y + GetComponent<Collider2D>().bounds.extents.y, 0);
        transform.position = snail.position + offset;

        transform.SetParent(snail);
        rb.velocity = snailRb.velocity;
        rb.isKinematic = false;

        Debug.Log("Attached to snail!");
    }

    void DetachFromSnail()
    {
        if (!isAttached) return;

        isAttached = false;
        transform.SetParent(null);
        rb.isKinematic = false;

        // ✅ Always allow jumping when detaching, even if snail is dead
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        Debug.Log("Detached from snail!");
    }

    public bool IsAttachedToSnail()
    {
        return isAttached;
    }
}
