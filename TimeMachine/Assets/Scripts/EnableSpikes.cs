using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSpikes : MonoBehaviour
{
    [SerializeField] private GameObject spike;
    private void OnTriggerEnter2D(Collider2D other) {
        spike.GetComponent<MovingSpikes>().StartMoving();
    }
}
