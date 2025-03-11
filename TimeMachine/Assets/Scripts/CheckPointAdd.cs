using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointAdd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if(TryGetComponent<BoxCollider2D>(out BoxCollider2D cd)){
                pos+=cd.offset;
            }
            // Debug.Log(pos);
            CheckpointManager.instance.SetPosition(pos);
        }
    }
}
