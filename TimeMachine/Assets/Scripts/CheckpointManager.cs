using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    private Vector2 position;
    public EventHandler OnResetObject;

    private void Awake()
    {
        // Singeton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        position = transform.position;
    }

    public void SetPosition(Vector2 pos){
        position = pos;
    }

    public Vector2 GetPosition(){
        return position;
    }


}
