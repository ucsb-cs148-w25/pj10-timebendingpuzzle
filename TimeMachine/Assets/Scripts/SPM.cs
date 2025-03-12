using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPM : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private CapsuleCollider2D coll;
    private Animator anim;
    private float dirX = 0f;
    bool dead = false;

    [SerializeField] private LayerMask jumpbleGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 10f;

    public float bounceForce;
    public float knockbackForce;
    public float knockbackCounter;
    public float knockbackTotalTime;
    public bool knockbackRight;

    public AudioSource Audio;
    public AudioClip jumpSound;
    public AudioClip moveSound;

    // Global variable to control starting orientation.
    // Default is true, meaning the player spawns facing right.
    [SerializeField] private bool startFacingRight = true;

    private enum MovementState { idle, running, jumping, falling }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Audio = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();

        // At spawn, set the sprite's facing based on startFacingRight.
        sprite.flipX = !startFacingRight;
    }

    void Update()
    {
        if (!dead)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                Audio.clip = jumpSound;
                Audio.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            UpdateAnimationState();
        }

        if (knockbackCounter > 0)
        {
            sprite.color = new Color(1f, 0f, 0f, 0.85f);
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    private void FixedUpdate()
    {
        if (knockbackCounter <= 0)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        else
        {
            if (knockbackRight)
            {
                rb.velocity = new Vector2(-knockbackForce, knockbackForce / 5);
            }
            else
            {
                rb.velocity = new Vector2(knockbackForce, knockbackForce / 5);
            }
            knockbackCounter -= Time.deltaTime;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false; // Moving right
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;  // Moving left
        }
        else
        {
            state = MovementState.idle;
            // When idle, we do NOT override sprite.flipX.
            // This way, the sprite will keep the last direction it was facing.
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpbleGround);
    }

    public void PlayMoveAudio()
    {
        Debug.Log("Playing Move Audio");
        Audio.clip = moveSound;
        Audio.Play();
    }

    public void Die()
    {
        dead = true;
        rb.velocity = Vector2.zero; // Stop any movement
        rb.bodyType = RigidbodyType2D.Static; // Completely disable physics
        coll.enabled = false; // Disable collider to prevent interactions
    }

    public Rigidbody2D GetRB()
    {
        return rb;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    [ContextMenu("Set Start Facing Left")]
    public void SetStartFacingLeft()
    {
        startFacingRight = false;
        if (sprite != null)
        {
            sprite.flipX = true;
        }
    }
}
