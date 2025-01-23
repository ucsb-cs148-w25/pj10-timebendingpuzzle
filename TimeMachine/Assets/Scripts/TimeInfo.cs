using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInfo
{
    //Variables to be recorded at each point in time
    private Vector2 Position;
    private Vector2 Velocity;
    // add state and rotation if needed
    private int AnimState;
    private bool FlipSprite;

    //Constructor for timeinfo
    public TimeInfo(Vector2 pos, Vector2 vel, int animationState, bool flipX)
    {
        this.Position = pos;
        this.Velocity = vel;
        this.AnimState = animationState;
        this.FlipSprite = flipX;
    }

    public Vector2 GetPosition(){
        return Position;
    }

    public Vector2 GetVelocity(){
        return Velocity;
    }

    public int GetAnimState(){
        return AnimState;
    }

    public bool GetSpriteFlip(){
        return this.FlipSprite;
    }


}
