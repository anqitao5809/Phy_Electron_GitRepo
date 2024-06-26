using UnityEngine;

public class ParticleMover : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;
    public Transform waypoint3;
    public Transform waypoint4;
    public Transform waypoint5;
    public Transform waypoint6;

    public float speed = 6.925f; // Speed calculated as total path length (55.4 units) divided by 8 seconds
    public float helixRadius = 1.25f; // Diameter is 2.5 units, so radius is 1.25 units
    public float helixTurns = 20f; // Total of 20 turns in the helix
    public float helixHeight = 5.1f; // Total height of the helix

    private int currentSegment = 0;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float journeyLength;
    private float startTime;
    private float helixLength;

    private void Start()
    {
        // Initialize the first segment
        InitializeSegment(waypoint1.position, waypoint2.position);

        // Calculate total helix length
        helixLength = helixTurns * 2f * Mathf.PI * helixRadius;
    }

    private void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        if (currentSegment == 1 || currentSegment == 2 || currentSegment == 4 || currentSegment == 5)
        {
            // Move along the straight line segments
            transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);
        }
        else if (currentSegment == 3)
        {
            float t = fractionOfJourney;

            // Calculate x and y coordinates based on helix path
            float theta = 2f * Mathf.PI * helixTurns * t;
            float x = helixRadius * Mathf.Cos(theta);
            float y = helixRadius * Mathf.Sin(theta);

            // Calculate z coordinate based on linear interpolation
            float zSpeed = helixHeight / helixTurns / helixLength; // Speed of z movement
            float z = -t * zSpeed * helixHeight; // Linearly interpolate z

            transform.position = new Vector3(x, y, z) + startPoint;
        }

        // Check if the segment is complete
        if (fractionOfJourney >= 1)
        {
            if (currentSegment == 1)
            {
                InitializeSegment(waypoint2.position, waypoint3.position);
            }
            else if (currentSegment == 2)
            {
                InitializeSegment(waypoint3.position, waypoint4.position);
            }
            else if (currentSegment == 3)
            {
                InitializeSegment(waypoint4.position, waypoint5.position);
            }
            else if (currentSegment == 4)
            {
                InitializeSegment(waypoint5.position, waypoint6.position);
            }
            else if (currentSegment == 5)
            {
                // Optionally loop or stop
                // InitializeSegment(waypoint1.position, waypoint2.position);
                enabled = false; // Stop the script
            }
        }
    }

    private void InitializeSegment(Vector3 start, Vector3 end)
    {
        currentSegment++;
        startPoint = start;
        endPoint = end;
        journeyLength = Vector3.Distance(startPoint, endPoint);
        startTime = Time.time;
    }
}
