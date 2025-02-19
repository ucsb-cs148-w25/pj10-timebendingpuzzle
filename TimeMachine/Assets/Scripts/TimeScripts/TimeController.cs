using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeController : MonoBehaviour
{
    private bool rewinding = false;
   
    private LinkedList<TimeInfo> rewindFrames = new LinkedList<TimeInfo>();
    private const int rewindTime = 8; // 8 seconds of rewinding time, if we choose to cap it
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    BoxCollider2D playerCollider;
    //private RewindCollisionCheck rewindCollisionChecker; 
    //private float collisionTime = 0.0f;
    private float collisionPenetrationThreshold = 0.1f;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //rewindCollisionChecker = new RewindCollisionCheck();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    void Update(){
        if(Input.GetKey(KeyCode.R)){
            rewinding = true; // TODO: implement event manager to tell other things to stop moving/doing their thing while we are rewinding
        } else{
            rewinding = false;
        }
    }

    void FixedUpdate(){
        if(rewinding == true && rewindFrames.Count > 0){
            rb.isKinematic = true; // for some unfathomable fucking reason collisions do not detect properly when physics are on. ???
                                    // probably because its kicking me out of being inside a block, which then gets stored in rewind frames,
                                    // but maybe theres disjunction between the frame thats added and the frame thats actually checked? IDK
            RewindStoredSeconds(); // if rewinding, we need to consume our stored time. FixedUpdate for consistency
        } else{
            StoreRewindSeconds(); // Store our points in time in FixedUpdate, so that large frame spikes dont reemulate that spike when rewinding.
            rb.isKinematic = false;
        }

    }

    void StoreRewindSeconds(){
        //while(rewindFrames.Count * Time.fixedDeltaTime >= rewindTime){ // while in case fixed intervals turn out to be a bad idea, then i can easily swap to normal timeupdates
        //    rewindFrames.RemoveFirst();
        //}
        if(rewindFrames.Count == 0 || rewindFrames.Last.Value.GetPosition() != (Vector2) transform.position){ // OR allowed since boolean logic ops are short circuiting
            TimeInfo timeInfo = new TimeInfo(transform.position, rb.velocity, anim.GetInteger("state"), sprite.flipX);
            rewindFrames.AddLast(timeInfo);
        }
    }

    void RewindStoredSeconds(){
        var collision = rewindCausesCollision(rewindFrames.Last.Value.GetPosition());
        if(!collision.Item1){
            rb.MovePosition(rewindFrames.Last.Value.GetPosition());
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            rewindFrames.RemoveLast();
        } 
        else{
            Debug.Log("Moving to safe pos");
            rb.MovePosition(collision.Item2);
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
        }
    }

    public (bool, Vector2) rewindCausesCollision(Vector2 predictedPosition){
        LayerMask layerMask = LayerMask.GetMask("Platform");
        float dist = Vector2.Distance(transform.position, predictedPosition);
        Vector2 dir = (predictedPosition - (Vector2) transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, playerCollider.size, transform.eulerAngles.z, dir, dist, layerMask);
        if(hits.Length > 0){
            foreach(RaycastHit2D hit in hits){
                if(hit.collider == playerCollider) continue;

                // Want to offset out hitpoint by the playercollider amt
                    // A collider will be colliding in only 1 direction. Cant next in corner, since first collision is counted. 
                    // Thus, only need to offset in the direction of the normal vector
                    // Doing it this way causes some maybe unexpected behavior when rewinding on a rotating object
                    // it sorta pushes you down instead of you being stuck to it, which i feel like is less than ideal but
                    // honestly its good enough. Possibly causes bad errors in future if it pushes you behind a wall through which you cant rewind?
                Vector2 newPos = hit.point + (hit.normal * playerCollider.size);
                    
                //Debug.DrawLine(hit.point, predictedPosition, Color.green);
                //Debug.Log("COLLIDING: OVERLAP");

                //Now, we want to check for the case that the colliding object is pushing us further away from the point that we want to rewind to
                    // We thus need to add more rewind frames
                if(Vector2.Distance(transform.position, predictedPosition) < Vector2.Distance(newPos, predictedPosition)){ // we are now further away while rewinding
                    StoreRewindSeconds();
                }
                return (true, newPos);
            }
        }
        //Debug.Log("NOT COLLIDING");
        return (false, Vector2.zero);
    }


    /*public (bool, Vector2, Vector2) rewindCausesCollision(Vector2 predictedPosition){ // returns bool, distance to colliding object, direction of colliding object
        //BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();

        /*List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Physics2D.OverlapCollider(playerCollider, filter, colliders);

        Vector2 size = playerCollider.size;
        //LayerMask layerMask = LayerMask.GetMask("Rewind Collision");
        LayerMask layerMask = LayerMask.GetMask("Platform");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
                                transform.position,
                                size,
                                transform.eulerAngles.z,
                                layerMask);

        foreach(Collider2D collider in colliders){
            if(collider == playerCollider) continue;

            ColliderDistance2D distance = playerCollider.Distance(collider); // issue: not using predicted posiition for collider
            if(distance.isOverlapped && Mathf.Abs(distance.distance) > collisionPenetrationThreshold){
                Debug.DrawLine(distance.pointA, distance.pointB, Color.green);
                return (true, distance.pointB, distance.normal);
            }
            //if(collider != playerCollider){
            //    Debug.Log("Causing a collision");
            //    return true;
            //}
        }
        return (false, Vector2.zero, Vector2.zero);
    }*/
    
}
