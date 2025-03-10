using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public Health playerHealth;
    public SPM playerMovement;
    
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
            playerMovement.knockbackCounter = playerMovement.knockbackTotalTime;
            if(collision.transform.position.x <= transform.position.x){
                playerMovement.knockbackRight = true;
            }
            if(collision.transform.position.x > transform.position.x){
                playerMovement.knockbackRight = false;
            }
            playerHealth.Attack();
        }
    }
}
