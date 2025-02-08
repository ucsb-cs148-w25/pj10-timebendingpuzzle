using System.Collections;
using System.Collections.Generic;
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
        if(rewinding == true){
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

        TimeInfo timeInfo = new TimeInfo(transform.position, rb.velocity, anim.GetInteger("state"), sprite.flipX);
        rewindFrames.AddLast(timeInfo);
    }

    void RewindStoredSeconds(){
        var collision = rewindCausesCollision(rewindFrames.Last.Value.GetPosition());
        if(rewindFrames.Count > 0 && !collision.Item1){
            /*transform.position = rewindFrames.Last.Value.GetPosition();
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            Debug.Log("Removing");
            Debug.Log(rewindFrames.Last.Value.GetPosition());
            rewindFrames.RemoveLast();*/
            rb.MovePosition(rewindFrames.Last.Value.GetPosition());
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            rewindFrames.RemoveLast();
        } 
        else if(collision.Item1){
            //TimeInfo timeInfo = new TimeInfo(rewindFrames.Last.Value.GetPosition(), rewindFrames.Last.Value.GetVelocity(), rewindFrames.Last.Value.GetAnimState(), rewindFrames.Last.Value.GetSpriteFlip());
            //rewindFrames.AddLast(timeInfo);
            //StoreRewindSeconds();
            //Vector2 dir = (rewindFrames.Last.Value.GetPosition() - (Vector2) transform.position).normalized;

            //transform.position = Vector2.Lerp(transform.position, collision.Item2 - dir * collisionPenetrationThreshold, 0.5f); // collision detected: lerp to nearest safe point
            
            if(Vector2.Distance(rewindFrames.Last.Value.GetPosition(), transform.position) > 0.2){
                Vector2 dir = (rewindFrames.Last.Value.GetPosition() - (Vector2)transform.position).normalized;
                Vector2 safePos = collision.Item2 - dir * collisionPenetrationThreshold;
                transform.position = safePos;
            }
            rb.velocity = Vector2.zero;
        } else{
            rewinding = false;
        }
    }

    public (bool, Vector2) rewindCausesCollision(Vector2 predictedPosition){
        //BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();

        /*List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Physics2D.OverlapCollider(playerCollider, filter, colliders);*/

        Vector2 size = playerCollider.size;
        LayerMask layerMask = LayerMask.GetMask("Rewind Collision");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
                                predictedPosition,
                                size,
                                transform.eulerAngles.z,
                                layerMask);

        foreach(Collider2D collider in colliders){
            if(collider == playerCollider) continue;

            ColliderDistance2D distance = playerCollider.Distance(collider);
            if(distance.isOverlapped && Mathf.Abs(distance.distance) > collisionPenetrationThreshold){
                Debug.DrawLine(distance.pointA, distance.pointB, Color.red);
                return (true, distance.pointB);
            }
            //if(collider != playerCollider){
            //    Debug.Log("Causing a collision");
            //    return true;
            //}
        }
        return (false, Vector2.zero);
    }
    
}
