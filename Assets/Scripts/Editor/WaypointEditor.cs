#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Waypoint waypoint = (Waypoint)target;

        if (GUILayout.Button("Generate New ID"))
        {
            Undo.RecordObject(waypoint, "Generate New Waypoint ID");
            waypoint.waypointID = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(waypoint);
        }
    }
}
#endif