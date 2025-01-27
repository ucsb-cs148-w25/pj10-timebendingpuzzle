using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimeController : MonoBehaviour
{
    private bool rewinding = false;
   
    private LinkedList<TimeInfo> rewindFrames = new LinkedList<TimeInfo>();
    private const int rewindTime = 8; // 8 seconds of rewinding time, if we choose to cap it
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        if(gameObject.GetComponent<Animator>() != null){
            anim = GetComponent<Animator>();
        }
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if(Input.GetKey(KeyCode.E)){
            rb.isKinematic = true;
            rewinding = true; // TODO: implement event manager to tell other things to stop moving/doing their thing while we are rewinding
        } else{
            rb.isKinematic = false;
            rewinding = false;
        }
    }

    void FixedUpdate(){
        if(rewinding == true){
            RewindStoredSeconds(); // if rewinding, we need to consume our stored time. FixedUpdate for consistency
        } else{
            StoreRewindSeconds(); // Store our points in time in FixedUpdate, so that large frame spikes dont reemulate that spike when rewinding.
        }
    }

    void StoreRewindSeconds(){
        //while(rewindFrames.Count * Time.fixedDeltaTime >= rewindTime){ // while in case fixed intervals turn out to be a bad idea, then i can easily swap to normal timeupdates
        //    rewindFrames.RemoveFirst();
        //}
        TimeInfo timeInfo = new TimeInfo(transform.position, rb.velocity);
        timeInfo.setFlip(sprite.flipX);
        timeInfo.setRotation(transform.rotation);
        if(anim != null){
            timeInfo.setAnim(anim.GetInteger("state"));
        }
        rewindFrames.AddLast(timeInfo);
    }

    void RewindStoredSeconds(){
        if(rewindFrames.Count > 0){
            transform.position = rewindFrames.Last.Value.GetPosition();
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            if(anim != null){
                anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            }
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            transform.rotation = rewindFrames.Last.Value.GetRotation();
            rewindFrames.RemoveLast();
        } else{
            rb.isKinematic = false;
            rewinding = false;
        }
    }
}
