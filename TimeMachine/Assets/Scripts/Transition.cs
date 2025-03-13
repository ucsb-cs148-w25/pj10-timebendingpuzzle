using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public GameObject levelEndPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject levelEndInstance = Instantiate(levelEndPrefab);
        other.gameObject.GetComponent<SPM>().isStop = true;
        
        Transform canvas = levelEndInstance.transform.Find("Canvas");
        if (canvas != null)
        {
            canvas.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas not found in prefab!");
        }

        Level_End_Invoke levelEndMenu = levelEndInstance.GetComponent<Level_End_Invoke>();
        if (levelEndMenu != null)
            levelEndMenu.Initialize(other.GetComponent<Health>().currLives);
        else
            Debug.LogError("LevelEndMenu script not found on prefab!");
    }
}
