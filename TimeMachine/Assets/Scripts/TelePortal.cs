using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortal : MonoBehaviour
{
    public SPM playerMovement;
    public TelePortal destination;
    public float pause;
    private bool inActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && inActive == false){
            Freeze();
            destination.Freeze();
            collision.transform.position = destination.GetPos();
        }
    }

    public Vector2 GetPos(){
        return transform.position;
    }

    public void Freeze(){
        inActive = true;
        pause = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if(pause > 0f && inActive){
            pause -= Time.deltaTime;
        }
        else if(pause <= 0f){
            inActive = false;
        }
        
    }
}
