using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(Back);
    }

    private void Back(){
        canvas.SetActive(false);
    }
}
