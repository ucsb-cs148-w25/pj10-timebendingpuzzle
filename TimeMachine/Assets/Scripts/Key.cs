using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private bool isPickUp = false;
    private GameObject player;
    private SpriteRenderer playerRenderer;
    private SpriteRenderer keyRenderer;
    [SerializeField] private float offset = 0.7f;
    private Vector3 originalPosition;
    private void Start() {
        keyRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        if(CheckpointManager.instance){
            CheckpointManager.instance.OnRestObject += ResetKey;
        }
        
    }

    private void Update()
    {
        if(isPickUp){
            followPlayer();
        }
    }

    private void followPlayer(){
        keyRenderer.flipY = playerRenderer.flipX;
        Vector3 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x + (playerRenderer.flipX?-offset:offset), playerPosition.y,playerPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            player = other.gameObject;
            isPickUp = true;
            playerRenderer = other.gameObject.GetComponent<SpriteRenderer>();
        }
        if(other.CompareTag("Door")){
            other.gameObject.GetComponent<Door>().openDoor();
            Destroy(gameObject);
        }
    }

    private void ResetKey(object sender, EventArgs e){
        isPickUp = false;
        transform.position = originalPosition;
        keyRenderer.flipY = false;
    }

    private void OnDestroy() {
        CheckpointManager.instance.OnRestObject -= ResetKey;
    }
}
