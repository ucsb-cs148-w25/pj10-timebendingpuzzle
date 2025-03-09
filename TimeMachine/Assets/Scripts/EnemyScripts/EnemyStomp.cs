using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    public SPM playerMovement;

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Weak Point"){
            Destroy(collision.gameObject);
            playerMovement.GetRB().velocity = new Vector2(10, playerMovement.GetJumpForce() * 1.25f);
        }
    }
}
