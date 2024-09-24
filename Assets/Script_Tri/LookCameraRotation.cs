using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    public PathManager pathManager; // Reference to the PathManager script
    public float rotationDamping = 5f; // Damping value for smooth rotation
    public bool useSideView = true; // Toggle option to switch between side view and direct direction

    private float targetYRotation; // Store the target Y-axis rotation

    void Update()
    {
        // Update the camera Y-axis rotation based on the current movement direction along the waypoints
        if (useSideView)
        {
            UpdateCameraSideViewRotation();
        }
        else
        {
            UpdateCameraDirectViewRotation();
        }
    }

    // Update camera to face the direction of movement (direct view)
    void UpdateCameraDirectViewRotation()
    {
        if (pathManager != null && pathManager.currentWaypoint != null && pathManager.nextWaypoint != null)
        {
            // Calculate the direction between current and next waypoint
            Vector3 directionToNextWaypoint = (pathManager.nextWaypoint.transform.position - pathManager.currentWaypoint.transform.position).normalized;

            // Calculate the Y-axis rotation angle (facing along the XZ plane) from the movement direction
            targetYRotation = Mathf.Atan2(directionToNextWaypoint.x, directionToNextWaypoint.z) * Mathf.Rad2Deg;

            // Get the current rotation's X and Z values, leaving them unchanged
            float currentXRotation = virtualCamera.transform.rotation.eulerAngles.x;
            float currentZRotation = virtualCamera.transform.rotation.eulerAngles.z;

            // Apply smooth Y-axis rotation while maintaining X and Z
            Quaternion targetRotation = Quaternion.Euler(currentXRotation, targetYRotation, currentZRotation);
            virtualCamera.transform.rotation = Quaternion.Slerp(
                virtualCamera.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationDamping // Apply damping for smooth Y-axis rotation
            );
        }
    }

    // Update camera to a side view of the movement direction (perpendicular view)
    void UpdateCameraSideViewRotation()
    {
        if (pathManager != null && pathManager.currentWaypoint != null && pathManager.nextWaypoint != null)
        {
            // Calculate the direction between current and next waypoint
            Vector3 directionToNextWaypoint = (pathManager.nextWaypoint.transform.position - pathManager.currentWaypoint.transform.position).normalized;

            // Create a side view direction by getting the perpendicular vector to the movement direction
            Vector3 sideViewDirection = Vector3.Cross(directionToNextWaypoint, Vector3.up).normalized;

            // Get the current rotation's X and Z values, leaving them unchanged
            float currentXRotation = virtualCamera.transform.rotation.eulerAngles.x;
            float currentZRotation = virtualCamera.transform.rotation.eulerAngles.z;

            // Apply smooth Y-axis rotation for the side view while maintaining X and Z
            Quaternion targetRotation = Quaternion.Euler(currentXRotation, Mathf.Atan2(sideViewDirection.x, sideViewDirection.z) * Mathf.Rad2Deg, currentZRotation);
            virtualCamera.transform.rotation = Quaternion.Slerp(
                virtualCamera.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationDamping // Apply damping for smooth side view rotation
            );
        }
    }
}
