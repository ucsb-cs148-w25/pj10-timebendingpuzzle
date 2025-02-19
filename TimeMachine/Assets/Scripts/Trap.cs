using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float time = 3f;
    private bool isEnetered = false;
    private GameObject m_player;

    private void Awake() {
        m_player = gameObject;
    }

    private void Update() {
        time += Time.deltaTime;
        Health health = m_player.GetComponent<Health>();
        if (time > 3f && health && isEnetered)
        {
            time = 0;
            health.Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            isEnetered = true;
            m_player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            isEnetered = false;
        }
    }
}
