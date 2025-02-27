using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimeController : MonoBehaviour
{
    private bool rewinding = false;
    private LinkedList<ObjectTimeInfo> rewindFrames = new LinkedList<ObjectTimeInfo>();
    private const int rewindTime = 8; 
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private IRewindable rewindable;
    private const int rewindSpeedMultiplier = 3; 

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        rewindable = GetComponent<IRewindable>();

        Time.fixedDeltaTime = 0.005f;

        for (int i = 0; i < 20; i++) StoreRewindSeconds();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            if (rb && rb.bodyType != RigidbodyType2D.Static) rb.isKinematic = true;
            rewinding = true;
            StartCoroutine(FastRewindStart());
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (rb && rb.bodyType != RigidbodyType2D.Static) rb.isKinematic = false;
            rewinding = false;
        }
    }

    private void FixedUpdate()
    {
        if (rewinding)
        {
            for (int i = 0; i < rewindSpeedMultiplier && rewindFrames.Count > 0; i++) 
            {
                RewindStoredSeconds(); 
            }
        }
        else
        {
            StoreRewindSeconds();
        }
    }

    private void StoreRewindSeconds()
    {
        ObjectTimeInfo timeInfo = new ObjectTimeInfo(transform.position, rb ? rb.velocity : Vector2.zero);
        if (sprite) timeInfo.SetFlip(sprite.flipX);
        timeInfo.SetRotation(transform.rotation);
        rewindFrames.AddLast(timeInfo);
        rewindable?.SaveState();
    }

    private void RewindStoredSeconds()
    {
        if (rewindFrames.Count > 0)
        {
            var lastFrame = rewindFrames.Last.Value;

            transform.position = lastFrame.GetPosition();
            if (rb && rb.bodyType != RigidbodyType2D.Static) rb.velocity = lastFrame.GetVelocity();
            if (sprite) sprite.flipX = lastFrame.GetSpriteFlip();
            transform.rotation = lastFrame.GetRotation();
            rewindFrames.RemoveLast();
            rewindable?.RewindState(); // custom rewinding per object
        } else{
            if(rb && (rb.bodyType != RigidbodyType2D.Static)) rb.isKinematic = false;
            rewinding = false;
        }
    }

    private IEnumerator FastRewindStart()
    {
        for (int i = 0; i < 10 && rewindFrames.Count > 0; i++) 
        {
            RewindStoredSeconds();
            yield return null;
        }
    }
}
