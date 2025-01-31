using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    private void Start() {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    private void Click(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
