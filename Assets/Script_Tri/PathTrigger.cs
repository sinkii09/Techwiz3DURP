using UnityEngine;

public class PathTrigger : MonoBehaviour
{
    public bool isPathZone; // Set in Unity: true for path zones, false for free movement zones
    [SerializeField] bool isChangeCurrentWP = false;
    [SerializeField] Waypoint currentWPToChange;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PathManager pathManager = other.GetComponent<PathManager>();
            pathManager.SetPathTracking(isPathZone); // Activate/deactivate path following
            if (isChangeCurrentWP)
            {
                pathManager.SetCurrentWP(currentWPToChange);
            }                                        
        }
    }
}
