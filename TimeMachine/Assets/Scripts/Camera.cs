using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {
        if (player != null)  // ✅ Prevents error when player is missing
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Player reference is missing in Camera.cs. Make sure the player exists in the scene.");
        }
    }
}
