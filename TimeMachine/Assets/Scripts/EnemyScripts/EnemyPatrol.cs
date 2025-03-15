using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour, IRewindable
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    private int patrolDestination;
    public bool killed = false;
    private Rigidbody2D rb;
    private Collider2D col;

    //Item 1 is killed, Item 2 is isKinematic, Item 3 is velocity
    private LinkedList<Tuple<bool, bool, Vector2>> rewindHistory = new LinkedList<Tuple<bool, bool, Vector2>>();

    // Update is called once per frame
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    void Update()
    {   
        if(!killed && !Input.GetKey(KeyCode.Q)){
            transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
            col.enabled = true;
            Patrol();
        } else if(killed){
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            col.enabled = false;
            Die();
        }
        
    }

    void Patrol(){
        if(patrolDestination == 0){
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, patrolPoints[0].position) < .2f){
                transform.localScale = new Vector3(1, 1, 1);
                patrolDestination = 1;
            }
        }
        if(patrolDestination == 1){
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, patrolPoints[1].position) < .2f){
                transform.localScale = new Vector3(-1, 1, 1);
                patrolDestination = 0;
            }
        }
    }

    void Die(){
        float x = moveSpeed * (transform.position.x > patrolPoints[patrolDestination].position.x ? -1 : 1);
        rb.velocity = new Vector2(x, rb.velocity.y + 2.5f * Physics2D.gravity.y * Time.deltaTime);
    }

    public void SaveState()
    {
        rewindHistory.AddLast(new Tuple<bool, bool, Vector2>(killed, rb.isKinematic, rb.velocity));
    }

    public void RewindState()
    {
        var stateInfo = rewindHistory.Last.Value;
        killed = stateInfo.Item1;
        //rb.isKinematic = stateInfo.Item2;
        rb.velocity = stateInfo.Item3;
        rewindHistory.RemoveLast();
    }

    public void ClearHistory()
    {
        rewindHistory.Clear();
    }
}
