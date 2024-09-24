using UnityEngine;

public class VFXController : MonoBehaviour
{
    // Reference to your VFX GameObject
    public Transform spawnPoint;
    public GameObject vfxEffect; // The VFX prefab or object

    // Method to trigger the VFX
    public void PlayVFX()
    {
        // Check if the VFX is already assigned
        if (vfxEffect != null)
        {
            // Instantiate the VFX at the object's position and rotation
            GameObject vfxInstance = Instantiate(vfxEffect, spawnPoint.position, transform.rotation);

            // Optional: Destroy the VFX after it plays (depends on the type of VFX)
            Destroy(vfxInstance, 2.0f); // Adjust the time based on VFX duration
        }
    }

    // Optional: Method to stop the VFX if needed
    public void StopVFX()
    {
        if (vfxEffect != null)
        {
            // If you want to disable or stop VFX, you can deactivate it here
            vfxEffect.SetActive(false);
        }
    }
}
