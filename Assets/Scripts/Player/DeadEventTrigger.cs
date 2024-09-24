using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEventTrigger : MonoBehaviour
{
    PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller  = GetComponentInParent<PlayerController>();
    }

    public void DeadEvent()
    {
        controller.DeadAnimationEvent();
    }
}
