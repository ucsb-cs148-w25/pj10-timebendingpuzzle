using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; 
    public float height = 3f; 
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Move the platform up and down
        float newY = startPos.y + Mathf.PingPong(Time.time * speed, height * 2) - height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
