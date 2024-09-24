using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            // Use ClosestPoint to get the point of impact relative to the player position
            Vector3 impactPoint = other.ClosestPoint(transform.position);
            if (!playerController.isInvincible)
                playerController.TrapTriggerAction(impactPoint); // Pass the position from trigger
        }
    }
}
