using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject manual;
    private void Awake() {
        menu.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(menu.activeSelf && !manual.activeSelf){
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
