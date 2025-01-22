using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPM : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private Animator anim;
    private float dirX = 0f;

    [SerializeField] private LayerMask jumpbleGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 10f;

    private enum MovementState {idle, running, jumping, falling}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX*moveSpeed,rb.velocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded()){
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState(){
        MovementState state;
        if(dirX > 0f){
            state = MovementState.running;
            sprite.flipX = false;
        }else if(dirX < 0f){
            state = MovementState.running;
            sprite.flipX = true;
        }else{
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f){
            state = MovementState.jumping;
        }else if(rb.velocity.y < -.1f){
            state = MovementState.falling;
        }

        anim.SetInteger("state",(int)state);
    }

    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center,coll.bounds.size, 0f, Vector2.down, .1f,jumpbleGround);
    }
}
