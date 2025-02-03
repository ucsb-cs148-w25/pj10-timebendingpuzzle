using UnityEngine;

public class TileMover : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement
    public float squareSize = 5f; // Size of the square pattern

    private Vector3[] path; // Array to store the path points
    private int currentPointIndex = 0; // Current target point in the path

    void Start()
    {
        // Define the square path
        path = new Vector3[]
        {
            //new Vector3(0, 0, 0), // Start point
            new Vector3(squareSize, 0, 0), // Right
            //new Vector3(squareSize, -squareSize, 0), // Down
            new Vector3(0, 0, 0), // Left
            //new Vector3(0, 0, 0) // Back to start
        };
    }

    void Update()
    {
        // Move towards the current target point
        transform.position = Vector3.MoveTowards(transform.position, path[currentPointIndex], moveSpeed * Time.deltaTime);

        // Check if the Tilemap has reached the current target point
        if (Vector3.Distance(transform.position, path[currentPointIndex]) < 0.01f)
        {
            // Move to the next point in the path
            currentPointIndex = (currentPointIndex + 1) % path.Length;
        }
    }
}