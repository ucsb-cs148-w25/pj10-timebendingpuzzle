using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour, IRewindable
{
    private Vector3 velocity = new Vector2(6f, 0f);
    public int dir;
    private float lifetime;
    private bool collided = false;
    public SPM playerMovement;
    private LaunchProjectile turret;

    private LinkedList<Tuple<float, bool, Vector2>> projectileHistory = new LinkedList<Tuple<float, bool, Vector2>>(); //contains lifetime of object

    void Start(){
        velocity *= dir;
        lifetime = 0;
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;

        turret = transform.parent.GetComponent<LaunchProjectile>();
    }

    void Update()
    {
        if(collided){
            velocity.y += 2.5f * Physics2D.gravity.y * Time.deltaTime; 
        }
        transform.Translate(velocity * Time.deltaTime);
        lifetime += Time.deltaTime;
        if(lifetime >= 10){
            if (turret != null && turret.activeProjectile == this.gameObject)
            {
                turret.activeProjectile = null;
            }
            Destroy(gameObject); //StartCoroutine(Kill());
        }
    }

    void IRewindable.SaveState()
    {
        projectileHistory.AddLast(new Tuple<float, bool, Vector2>(lifetime, collided, velocity));
    }

    void IRewindable.RewindState()
    {
        lifetime = projectileHistory.Last.Value.Item1;
        collided = projectileHistory.Last.Value.Item2; 
        velocity = projectileHistory.Last.Value.Item3;
        projectileHistory.RemoveLast();
        if(projectileHistory.Count == 0){
            Destroy(gameObject);
        }
    }

    void IRewindable.ClearHistory()
    {
        projectileHistory.Clear();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        Vector2 direction = velocity;

        foreach (ContactPoint2D contact in contacts) {
            direction = (Vector2) transform.position - contact.point;
            direction.Normalize();
        }

        if(!collided){
            Debug.Log("COLLIDED");
            velocity.x = velocity.x * dir * direction.x;
            collided = true;
            if (turret != null && turret.activeProjectile == this.gameObject)
            {
                turret.activeProjectile = null;
            }
            //StartCoroutine(Kill());
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player")){
            //playerMovement.GetRB().velocity = new Vector2(10, playerMovement.GetJumpForce() * 1.25f);
            playerMovement.Bounce();
            collided = true;
            if (turret != null && turret.activeProjectile == this.gameObject)
            {
                turret.activeProjectile = null;
            }
        }
    }

}
