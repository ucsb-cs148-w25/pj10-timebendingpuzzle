using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Level5_End : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
