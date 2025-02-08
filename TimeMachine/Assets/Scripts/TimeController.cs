using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private bool rewinding = false;
   
    private LinkedList<TimeInfo> rewindFrames = new LinkedList<TimeInfo>();
    private const int rewindTime = 8; // 8 seconds of rewinding time, if we choose to cap it
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public Image playerRevArrow;
    private RectTransform playerRectTransform;
    public float revArrowRot = 10f;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerRectTransform = playerRevArrow.GetComponent<RectTransform>();
    }

    void Update(){
        if(Input.GetKey(KeyCode.R)){
            rewinding = true; // TODO: implement event manager to tell other things to stop moving/doing their thing while we are rewinding
        } else{
            rewinding = false;
        }

        if(rewinding){
            playerRectTransform.Rotate(0, 0, revArrowRot);
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
        TimeInfo timeInfo = new TimeInfo(transform.position, rb.velocity, anim.GetInteger("state"), sprite.flipX);
        rewindFrames.AddLast(timeInfo);
    }

    void RewindStoredSeconds(){
        if(rewindFrames.Count > 0){
            transform.position = rewindFrames.Last.Value.GetPosition();
            rb.velocity = rewindFrames.Last.Value.GetVelocity();
            anim.SetInteger("state", rewindFrames.Last.Value.GetAnimState());
            sprite.flipX = rewindFrames.Last.Value.GetSpriteFlip();
            rewindFrames.RemoveLast();
        } else{
            rewinding = false;
        }
    }
}
