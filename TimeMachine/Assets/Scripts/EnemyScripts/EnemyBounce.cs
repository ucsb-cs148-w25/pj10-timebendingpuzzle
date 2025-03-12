using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : MonoBehaviour
{
    public SPM playerMovement;
    public float bounceForce;
    
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Player"){
            playerMovement.bounceForce = bounceForce;
        }
    }
}
