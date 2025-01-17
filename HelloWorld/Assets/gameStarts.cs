using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameStarts : MonoBehaviour
{
    public void function1(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void toMainPage(){
        SceneManager.LoadScene(0);
    }
}
