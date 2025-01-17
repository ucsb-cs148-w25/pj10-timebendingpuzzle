using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    [SerializeField]GameObject helloPage;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClickEvent);
    }

    void ButtonClickEvent(){
        if(helloPage.activeSelf == false)
            helloPage.SetActive(true);
        else{
            helloPage.SetActive(false);
        }
    }
}
