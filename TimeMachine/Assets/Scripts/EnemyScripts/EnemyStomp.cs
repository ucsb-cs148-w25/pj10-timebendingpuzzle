using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    public SPM playerMovement;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Weak Point"){
            Destroy(collision.gameObject);
            playerMovement.Bounce();
        }
    }
}
