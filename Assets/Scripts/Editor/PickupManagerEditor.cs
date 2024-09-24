using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickupManagerEditor : EditorWindow
{
    [MenuItem("Tools/Pickup Manager")]
    public static void ShowWindow()
    {
        GetWindow<PickupManagerEditor>("Pickup Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("Pickup Manager", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate All Pickup IDs"))
        {
            GenerateAllWaypointIDs();
        }

        if (GUILayout.Button("Clear All Pickup IDs"))
        {
            ClearAllWaypointIDs();
        }
    }

    private void GenerateAllWaypointIDs()
    {
        PickupItem[] allWaypoints = FindObjectsOfType<PickupItem>();

        foreach (PickupItem waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Generate Waypoint ID");
            waypoint.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Generated IDs for {allWaypoints.Length} waypoints.");
    }

    private void ClearAllWaypointIDs()
    {
        PickupItem[] allWaypoints = FindObjectsOfType<PickupItem>();

        foreach (PickupItem waypoint in allWaypoints)
        {
            Undo.RecordObject(waypoint, "Clear CheckPoint ID");
            waypoint.id = "";
            EditorUtility.SetDirty(waypoint);
        }

        Debug.Log($"Cleared IDs for {allWaypoints.Length} waypoints.");
    }
}
