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
    public AudioSource Audio;
    public AudioClip jumpSound;
    public AudioClip moveSound;

    private enum MovementState {idle, running, jumping, falling}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Audio = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dirX*moveSpeed,rb.velocity.y);



            if (Input.GetButtonDown("Jump") && IsGrounded()){
                Audio.clip = jumpSound;
                Audio.Play();
                rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            }

            UpdateAnimationState();
        }
    }

    private void UpdateAnimationState(){
        MovementState state;
        if(dirX > 0f){
            state = MovementState.running;
            // if(moveAudio.isPlaying == false)
            //     moveAudio.Play();
            sprite.flipX = false;
        }else if(dirX < 0f){
            state = MovementState.running;
            // if(moveAudio.isPlaying == false)
            //     moveAudio.Play();
            sprite.flipX = true;
        }else{
            state = MovementState.idle;
            // moveAudio.Stop();
        }
       

        if(rb.velocity.y > .1f){
            state = MovementState.jumping;
            // moveAudio.Stop();
        }else if(rb.velocity.y < -.1f){
            state = MovementState.falling;
            // moveAudio.Stop();
        }

        anim.SetInteger("state",(int)state);
    }

    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size, 0f, Vector2.down, .1f,jumpbleGround);
    }

    public void PlayMoveAudio(){
        Debug.Log("Playing Move Audio");
        Audio.clip = moveSound;
        Audio.Play();
    }

    public void Die() {
        dead = true;
        rb.velocity = Vector2.zero; // Stop any movement
        rb.bodyType = RigidbodyType2D.Static; // Completely disable physics
        coll.enabled = false; // Disable collider to prevent interactions
    }
}
