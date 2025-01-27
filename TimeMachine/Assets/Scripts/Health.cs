using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private List<GameObject> Lives;
    private float currLives;

    private void Start() 
    {
        currLives = 3;
    }

    private void Update() 
    {

    }

    public void Attack()
    {
        foreach (GameObject live in Lives)
        {
            if (live.activeSelf == true)
            {
                live.SetActive(false);
                currLives--;
                break;
            }
        }
    }

    public void Heal()
    {
        for (int i = Lives.Count - 1; i >= 0; i--)
        {
            if (Lives[i].activeSelf == false)
            {
                Lives[i].SetActive(true);
                currLives++;
                break;
            }
        }
    }
}
