using UnityEngine;

[System.Serializable] // This will allow Unity to show the class in the Inspector
public class WaypointPath
{
    public Transform startPoint;
    public Transform endPoint;

    // Constructor to set up the start and end points
    public WaypointPath(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
    }
}
