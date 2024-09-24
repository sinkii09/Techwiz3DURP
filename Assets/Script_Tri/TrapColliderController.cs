using UnityEngine;

public class TrapColliderController : MonoBehaviour
{
    // Reference to the collider of the trap
    public Collider trapCollider;

    // Reference to the GameObject to follow
    public GameObject objectToFollow;

    void Start()
    {
        if (trapCollider == null)
        {
            trapCollider = GetComponent<Collider>();
        }

        if (objectToFollow == null)
        {
            Debug.LogError("No object to follow assigned!");
        }
    }

    void Update()
    {
        // Check if the objectToFollow is active
        if (objectToFollow != null)
        {
            // If the objectToFollow is active, enable the trap's collider
            trapCollider.enabled = objectToFollow.activeInHierarchy;
        }
    }
}
