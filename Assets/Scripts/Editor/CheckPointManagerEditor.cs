using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CheckPointManagerEditor : EditorWindow
{
    [MenuItem("Tools/CheckPoint Manager")]
    public static void ShowWindow()
    {
        GetWindow<CheckPointManagerEditor>("Checkpoint Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("Checkpoint Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate All Checkpoint IDs"))
        {
            GenerateAllWaypointIDs();
        }

        if (GUILayout.Button("Clear All Checkpoint IDs"))
        {
            ClearAllWaypointIDs();
        }
    }

    private void GenerateAllWaypointIDs()
    {
        CheckPoint[] allWaypoints = FindObjectsOfType<CheckPoint>();

        foreach (CheckPoint waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Generate Waypoint ID");
            waypoint.Id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Generated IDs for {allWaypoints.Length} waypoints.");
    }

    private void ClearAllWaypointIDs()
    {
        CheckPoint[] allWaypoints = FindObjectsOfType<CheckPoint>();

        foreach (CheckPoint waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Clear CheckPoint ID");
            waypoint.Id = "";
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Cleared IDs for {allWaypoints.Length} waypoints.");
    }
}
