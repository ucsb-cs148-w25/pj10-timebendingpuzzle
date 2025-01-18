using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float horizontalInput;
    private Vector3 jump;
    private float jumpForce = 2.0f;
    private bool isGrounded;
    Rigidbody2D rb;

    private Animator anim;
    private bool facingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = new Vector3(0.0f, jumpForce, 0.0f);
        anim = GetComponent<Animator>();
    }

    void OnCollisionStay2D(){
        isGrounded = true;
    }

    void Jump(){
        rb.AddForce(jump * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void Flip(){
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Move(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            Jump();
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if(horizontalInput != 0){
            anim.Play("run-Animation");
            if(horizontalInput > 0 && !facingRight) {
                Flip();
            } else if(horizontalInput < 0 && facingRight){
                Flip();
            } 
        }
        else{
            anim.Play("Idle-Animation");
        }
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
