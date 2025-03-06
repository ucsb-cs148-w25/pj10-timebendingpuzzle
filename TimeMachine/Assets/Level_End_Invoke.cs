using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_End_Invoke : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;
    public int playerHealth;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public void Initialize(int health)
    {
        playerHealth = health;
        star1.SetActive(playerHealth >= 1);
        star2.SetActive(playerHealth >= 2);
        star3.SetActive(playerHealth >= 3);
    }

}
