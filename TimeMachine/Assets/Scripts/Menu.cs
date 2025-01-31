using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private void Awake() {
        menu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(menu.activeSelf){
                menu.SetActive(false);
                Time.timeScale = 1;
            }
            else{
                menu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
