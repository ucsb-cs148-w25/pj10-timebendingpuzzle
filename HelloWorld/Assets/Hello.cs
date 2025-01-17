using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hello : MonoBehaviour
{
    // Start is called before the first frame update
    public void but(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
