using UnityEngine;
using UnityEditor;
using System.Linq;

public class WaypointManagerEditor : EditorWindow
{
    [MenuItem("Tools/Waypoint Manager")]
    public static void ShowWindow()
    {
        GetWindow<WaypointManagerEditor>("Waypoint Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("Waypoint Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate All Waypoint IDs"))
        {
            GenerateAllWaypointIDs();
        }

        if (GUILayout.Button("Clear All Waypoint IDs"))
        {
            ClearAllWaypointIDs();
        }
    }

    private void GenerateAllWaypointIDs()
    {
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Generate Waypoint ID");
            waypoint.waypointID = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Generated IDs for {allWaypoints.Length} waypoints.");
    }

    private void ClearAllWaypointIDs()
    {
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();

        foreach (Waypoint waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Clear Waypoint ID");
            waypoint.waypointID = "";
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Cleared IDs for {allWaypoints.Length} waypoints.");
    }
}