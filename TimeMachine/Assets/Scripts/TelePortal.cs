using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortal : MonoBehaviour
{
    public SPM playerMovement;
    public TelePortal destination;
    public float pause = 0f;
    private bool inActive = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player" && inActive == false){
            destination.Freeze();
            collision.transform.position = destination.GetPos();
        }
    }

    public Vector2 GetPos(){
        return transform.position;
    }

    public void Freeze(){
        inActive = true;
        pause = 10f;
    }
    // Update is called once per frame
    void Update()
    {
        while(pause > 0f){
            pause -= Time.deltaTime;
        }
        if(pause <= 0f){
            inActive = false;
        }
        
    }
}
