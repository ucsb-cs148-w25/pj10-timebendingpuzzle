using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Next_Level_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start() {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    private void Click(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
