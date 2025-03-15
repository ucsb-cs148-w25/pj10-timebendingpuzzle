using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform[] movePoints;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetKey(KeyCode.Q)){
            transform.position = Vector2.MoveTowards(transform.position, movePoints[1].position, moveSpeed * Time.deltaTime);
                if(Vector2.Distance(transform.position, movePoints[1].position) < .2f){
                    transform.position = movePoints[0].position;
                }
        }
        
    }
}
