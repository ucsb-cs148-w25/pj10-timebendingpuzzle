using UnityEngine;

public class ProjectileStomp : MonoBehaviour
{
    public SPM playerMovement;

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Weak Point"){
            //playerMovement.GetRB().velocity = new Vector2(10, playerMovement.GetJumpForce() * 1.25f);
        }
    }
}
