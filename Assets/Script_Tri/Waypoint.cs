using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public string waypointID;
    public Waypoint previousWaypoint; // The previous waypoint in the path
    public Waypoint nextWaypoint;     // The next waypoint in the path
}
