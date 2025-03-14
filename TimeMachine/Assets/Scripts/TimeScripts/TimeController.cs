
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class TimeController : MonoBehaviour
{
    private bool rewinding = false;
   
    private LinkedList<TimeInfo> rewindFrames = new LinkedList<TimeInfo>();
    private const int rewindTime = 8; // 8 seconds of rewinding time, if we choose to cap it
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    CapsuleCollider2D playerCollider;

    //bool checkpoint = false;
    //private RewindCollisionCheck rewindCollisionChecker; 
    //private float collisionTime = 0.0f;
    //private float collisionPenetrationThreshold = 0.1f;

    public Image pRevArrow;
    private RectTransform pRevArrowTransform;
    public float pRevArrowRot = 10f;
    private float pOscSpeed = 10f;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //rewindCollisionChecker = new RewindCollisionCheck();
        playerCollider = GetComponent<CapsuleCollider2D>();
        pRevArrowTransform = pRevArrow.GetComponent<RectTransform>();
    }

    void Update(){
        if(Input.GetKey(KeyCode.R)){
            rewinding = true; // TODO: implement event manager to tell other things to stop moving/doing their thing while we are rewinding
        } else{
            rewinding = false;
        }

        if(rewinding){
            float colorOsc = (Mathf.Sin(Time.time * pOscSpeed) + 1f) / 2f;
            pRevArrowTransform.Rotate(0, 0, pRevArrowRot);
            sprite.color = Color.Lerp(Color.blue, Color.white, colorOsc);
        }
        else{
            sprite.color = Color.white;
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
            //timeInfo.SetCheckpoint(checkpoint);
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
            //Debug.Log("Moving to safe pos");
            rb.MovePosition(collision.Item2);
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
        }
    }

    public (bool, Vector2) rewindCausesCollision(Vector2 predictedPosition){
        LayerMask layerMask = LayerMask.GetMask("Ground");
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

    public void CheckpointReset(){
        rewindFrames.Clear();
    }

    //void checkpointReset(){
    //    while(rewindFrames.Count > 0 && !rewindFrames.Last.Value.GetCheckpoint()){
    //        rewindFrames.RemoveLast();
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D other){
    //    if(other.CompareTag("Checkpoint")){
    //        checkpoint = true;
    //    }
    //}
//
    //void OnTriggerExit2D(Collider2D other){
    //    if(other.CompareTag("Checkpoint")){
    //        checkpoint = false;
    //    }
    //}
    
}
