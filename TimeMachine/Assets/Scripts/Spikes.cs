using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the spikes!");

            // Attempt to get the Player script and call Die() if it exists
            SPM player = collision.gameObject.GetComponent<SPM>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}