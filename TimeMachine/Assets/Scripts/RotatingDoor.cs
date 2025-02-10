using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDoor : MonoBehaviour, IRewindable
{
    private float targetRotationZ;
    private bool isRotating = false;
    private float counter = 0f;
    private bool isClockwise = false;


    // Rotation speed
    public float rotationSpeed = 5f;
    
    //rewind history
    private LinkedList<Tuple<bool, float>> rotationHistory = new LinkedList<Tuple<bool, float>>();
    private void Start()
    {
        targetRotationZ = transform.eulerAngles.z; // Initial rotation
    }

    public void RotateDoor()
    {
        if (!isRotating)
        {
            if (isClockwise == false){
            counter += 1f;
            targetRotationZ += 90f; 
            StartCoroutine(RotateToTarget());

            }

            if (isClockwise == true){
            counter -= 1f;
            targetRotationZ -= 90f; 
            StartCoroutine(RotateToTarget());
            }

            if (counter == 2f){
                    isClockwise = true;
                }
            if (counter == 0f){
                    isClockwise = false;
                }
        }
    }

    private System.Collections.IEnumerator RotateToTarget()
    {
        isRotating = true;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetRotationZ)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            float newZ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotationZ, step);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZ);
            yield return null;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetRotationZ);
        isRotating = false;
    }

    public void SaveState(){
        rotationHistory.AddLast(Tuple.Create(isRotating, targetRotationZ));
    }

    public void RewindState(){
        if(rotationHistory.Last.Value.Item1 == false){ // if in  the most recent saved timepoint, the door WASNT rotating
            targetRotationZ = rotationHistory.Last.Value.Item2;
        } else if(!isRotating){ // if door isnt currently rotating, but the last saved timepoint door was rotating
            StartCoroutine(RotateToTarget());
        }
        rotationHistory.RemoveLast();
    }
}