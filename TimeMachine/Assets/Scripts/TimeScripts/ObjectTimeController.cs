using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectTimeController : MonoBehaviour
{
    private bool rewinding = false;
   
    private LinkedList<ObjectTimeInfo> rewindFrames = new LinkedList<ObjectTimeInfo>();
    private const int rewindTime = 8; // 8 seconds of rewinding time, if we choose to cap it
    private Rigidbody2D rb;
    //privateted Animator anim;
    private SpriteRenderer sprite;
    private IRewindable rewindable;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rewindable = GetComponent<IRewindable>();
    }

    private void Update(){
        if(Input.GetKey(KeyCode.Q)){
            if(rb && (rb.bodyType != RigidbodyType2D.Static)) rb.isKinematic = true;
            rewinding = true; // TODO: implement event manager to tell other things to stop moving/doing their thing while we are rewinding
        } else{
            if(rb && (rb.bodyType != RigidbodyType2D.Static)) rb.isKinematic = false;
            rewinding = false;
        }
    }

    private void FixedUpdate(){
        if(rewinding == true){
            RewindStoredSeconds(); // if rewinding, we need to consume our stored time. FixedUpdate for consistency
        } else{
            StoreRewindSeconds(); // Store our points in time in FixedUpdate, so that large frame spikes dont reemulate that spike when rewinding.
        }
    }

    private void StoreRewindSeconds(){
        //while(rewindFrames.Count * Time.fixedDeltaTime >= rewindTime){ // while in case fixed intervals turn out to be a bad idea, then i can easily swap to normal timeupdates
        //    rewindFrames.RemoveFirst();
        //}
        ObjectTimeInfo timeInfo = new ObjectTimeInfo(transform.position, rb ? rb.velocity : Vector2.zero);
        if (sprite) timeInfo.setFlip(sprite.flipX);
        timeInfo.setRotation(transform.rotation);
        rewindFrames.AddLast(timeInfo);
        //if(anim != null){
        //    timeInfo.setAnim(anim.GetInteger("state"));
        //}
        //rewindFrames.AddLast(timeInfo);
        rewindable?.SaveState(); // custom info, beyond position and velocity
    }

    private void RewindStoredSeconds(){
        if(rewindFrames.Count > 0){
            transform.position = rewindFrames.Last.Value.GetPosition();
            if(rb && (rb.bodyType != RigidbodyType2D.Static)) rb.velocity = rewindFrames.Last.Value.GetVelocity();
            //if(anim != null){
            //    anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            //}
            if(sprite) sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            transform.rotation = rewindFrames.Last.Value.GetRotation();
            rewindFrames.RemoveLast();
            rewindable?.RewindState(); // custom rewinding per object
        } else{
            if(rb) rb.isKinematic = false;
            rewinding = false;
        }
    }
}
