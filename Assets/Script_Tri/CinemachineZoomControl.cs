using UnityEngine;
using Cinemachine;

public class CinemachineZoomControl : MonoBehaviour
{
    public CinemachineFollowZoom followZoom; // Reference to the CinemachineFollowZoom
    public GameObject targetObject; // The object whose movement we will track (e.g., player)

    [Header("Zoom Width Settings")]
    [SerializeField] private float idleWidth = 8f; // Width when idle (zoom in)
    [SerializeField] private float movingWidth = 14f; // Width when moving (zoom out)

    [Header("Zoom Speed Settings")]
    [SerializeField] private float zoomInSpeed = 2f; // Speed at which the camera zooms in
    [SerializeField] private float zoomOutSpeed = 4f; // Speed at which the camera zooms out (faster)

    [Header("Ease Settings")]
    [SerializeField] private float easeFactor = 1f; // Controls the strength of the ease-in and ease-out

    private float targetWidth;
    private Vector3 previousPosition; // Track the previous position of the target object

    void Start()
    {
        if (followZoom == null)
        {
            followZoom = GetComponent<CinemachineFollowZoom>();
        }

        // Initialize to the idle width
        targetWidth = idleWidth;
        followZoom.m_Width = targetWidth;

        // Store the initial position of the target object
        if (targetObject != null)
        {
            previousPosition = targetObject.transform.position;
        }
    }

    void Update()
    {
        if (targetObject != null)
        {
            HandleZoomWidth();
        }
    }

    void HandleZoomWidth()
    {
        // Check if the target object is moving by comparing the current position with the previous position
        bool isMoving = (targetObject.transform.position - previousPosition).magnitude > 0.01f; // Small threshold to detect movement

        // Set the target width based on whether the object is moving or idle
        if (isMoving)
        {
            targetWidth = movingWidth; // Set to moving width
        }
        else
        {
            targetWidth = idleWidth; // Set to idle width
        }

        // Choose the speed depending on whether we're zooming in or out
        float zoomSpeed = (targetWidth > followZoom.m_Width) ? zoomOutSpeed : zoomInSpeed;

        // Calculate the easing value (Ease-in and Ease-out effect)
        float t = Mathf.Clamp01((Mathf.Abs(targetWidth - followZoom.m_Width) / Mathf.Abs(movingWidth - idleWidth)) * easeFactor);
        t = EaseInOutCubic(t); // Apply ease-in and ease-out using a cubic function

        // Smoothly interpolate the current width towards the target width using easing
        followZoom.m_Width = Mathf.Lerp(followZoom.m_Width, targetWidth, t * Time.deltaTime * zoomSpeed);

        // Update the previous position to the current position
        previousPosition = targetObject.transform.position;
    }

    // Easing function for ease-in and ease-out
    float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}
