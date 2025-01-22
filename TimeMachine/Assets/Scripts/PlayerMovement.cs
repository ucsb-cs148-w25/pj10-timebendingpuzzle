using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private float m_horizontalMovement;
    private float m_curJump = 0;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private Transform m_groudCheck;
    public LayerMask m_layerMask;
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsGround()) m_curJump = 0;
        m_horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && (IsGround() || m_curJump < 1))
        {
            m_curJump++;
            jump();
        }
    }
    private void FixedUpdate() 
    {
        m_rigidBody.velocity = new Vector2(m_horizontalMovement * m_speed, m_rigidBody.velocity.y);
    }

    private void jump()
    {
        m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, m_jumpForce);
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCircle(m_groudCheck.position, 0.2f, m_layerMask);
    }
}
