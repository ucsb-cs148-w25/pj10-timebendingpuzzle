using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectTimeController2 : MonoBehaviour
{
    private bool rewinding = false;
    private LinkedList<ObjectTimeInfo> rewindFrames = new LinkedList<ObjectTimeInfo>();
    private const int rewindTime = 8; 
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private IRewindable rewindable;

    [SerializeField] private bool fastRewind = false;
    private int rewindSpeedMultiplier; 
    private float rewindDuration = 0.08f; // Adjust for smoother or faster rewinding

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        rewindable = GetComponent<IRewindable>();

        Time.fixedDeltaTime = 0.005f;
        
        if(fastRewind){
            rewindSpeedMultiplier = 2;
            for (int i = 0; i < 20; i++) StoreRewindSeconds();
        } else rewindSpeedMultiplier = 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            if (rb && rb.bodyType != RigidbodyType2D.Static) rb.isKinematic = true;
            rewinding = true;
            if(fastRewind) StartCoroutine(FastRewindStart());
        }
        else
        {
            if (rb && rb.bodyType != RigidbodyType2D.Static) rb.isKinematic = false;
            rewinding = false;
        }
    }

    private void FixedUpdate()
    {
        if (rewinding)
        {
            for (int i = 0; i < rewindSpeedMultiplier && rewindFrames.Count > 1; i++) 
            {
                StartCoroutine(SmoothRewind());
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

    private IEnumerator SmoothRewind()
    {
        if (rewindFrames.Count > 1)
        {
            var lastFrame = rewindFrames.Last.Value;
            var secondLastFrame = rewindFrames.Last.Previous.Value; // Get previous frame for interpolation

            float elapsedTime = 0f;
            Vector2 startPosition = lastFrame.GetPosition();
            Vector2 endPosition = secondLastFrame.GetPosition();

            Quaternion startRotation = lastFrame.GetRotation();
            Quaternion endRotation = secondLastFrame.GetRotation();

            Vector2 startVelocity = lastFrame.GetVelocity();
            Vector2 endVelocity = secondLastFrame.GetVelocity();

            while (elapsedTime < rewindDuration)
            {
                float t = elapsedTime / rewindDuration;
                transform.position = Vector2.Lerp(startPosition, endPosition, t);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                if (rb && rb.bodyType != RigidbodyType2D.Static) 
                    rb.velocity = Vector2.Lerp(startVelocity, endVelocity, t);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
            transform.rotation = endRotation;
            if (rb && rb.bodyType != RigidbodyType2D.Static) 
                rb.velocity = endVelocity;

            rewindFrames.RemoveLast();
            rewindable?.RewindState();
        }
        else
        {
            if (rb) rb.isKinematic = false;
            rewinding = false;
        }
    }

    private IEnumerator FastRewindStart()
    {
        for (int i = 0; i < 10 && rewindFrames.Count > 0; i++) 
        {
            StartCoroutine(SmoothRewind());
            yield return null;
        }
    }

    public void CheckpointReset(){
        rewindFrames.Clear();
        rewindable?.ClearHistory();
    }
}
