using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
            // Call the ApplyKnockback function when colliding with a trap
            other.GetComponent<PathManager>().ApplyKnockback(transform.position, 1f); // Use a small force like 2f for knockback
        }
    }
}
