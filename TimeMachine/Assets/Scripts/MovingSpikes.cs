using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public float speed = 5;
    public bool isMoving = false;
    private Rigidbody2D rb;
    private Vector3 origin;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        origin = transform.position;

        // StartMoving();
    }
    private void Start() {
        CheckpointManager.instance.OnResetObject += ResetSpikes;
    }

    private void OnDestroy() {
        CheckpointManager.instance.OnResetObject -= ResetSpikes;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMoving && origin == transform.position)   isMoving = false;
        
    }

    public void StartMoving(){
        isMoving = true;
        rb.velocity = new Vector2(speed, 0);
    }

    public void StopMoving(){
        isMoving = false;
        rb.velocity = new Vector2(0, 0);
    }

    private void ResetSpikes(object sender, EventArgs e){
        //clean object time stack
        StopMoving();
        transform.position = origin;
        
    }
}
