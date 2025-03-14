using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Assign your player character here
    public float fixedYPosition = 0f;
    public float followSpeed = 5f;  // smoother movement
    void LateUpdate()
    {
        if (target != null)
        {
            // Only follow the target's X position, keeping Y fixed
            Vector3 newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime);
        }
    }
}