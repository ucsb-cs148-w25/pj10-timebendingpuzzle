using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualButton : MonoBehaviour
{
    [SerializeField] private GameObject manualCanvas;

    private void Awake() {
        manualCanvas.SetActive(false);
        GetComponent<Button>().onClick.AddListener(Manual);
    }

    private void Manual(){
        manualCanvas.SetActive(true);
    }
}
