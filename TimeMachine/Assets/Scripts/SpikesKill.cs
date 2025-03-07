using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikesKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            other.GetComponent<Health>().Attack();
            // Debug.Log(CheckpointManager.instance.GetPosition());
            other.transform.position = new Vector3(CheckpointManager.instance.GetPosition().x, CheckpointManager.instance.GetPosition().y);
            //reset player time stack;
            CheckpointManager.instance.OnRestObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
