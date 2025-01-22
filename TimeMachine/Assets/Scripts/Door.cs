using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject closedDoor;
    private void Start()
    {
        closedDoor.SetActive(false);
    }

    private void openDoor(){
        closedDoor.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Key")){
            openDoor();
        }
    }
}
