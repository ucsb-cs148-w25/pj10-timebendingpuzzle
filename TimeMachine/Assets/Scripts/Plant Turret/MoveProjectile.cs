using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveProjectile : MonoBehaviour, IRewindable
{
    private Vector3 velocity = new Vector2(10f, 0f);
    public int dir;
    private float lifetime;
    private bool collided = false;

    private LinkedList<Tuple<float, bool, Vector2>> projectileHistory = new LinkedList<Tuple<float, bool, Vector2>>(); //contains lifetime of object

    void Start(){
        velocity *= dir;
        lifetime = 0;
    }

    void Update()
    {
        if(!collided){
            gameObject.transform.Translate(velocity * Time.deltaTime);
            lifetime += Time.deltaTime;
            if(lifetime >= 5) Destroy(gameObject); //StartCoroutine(Kill());
        } else{
            velocity.y += 2.5f * Physics2D.gravity.y * Time.deltaTime; 
            transform.Translate(velocity * Time.deltaTime);
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
            //StartCoroutine(Kill());
        }
    }

}
