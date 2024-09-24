using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject tt1;

    public bool IsEnter;
    int i = -1;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (IsEnter)
            {
                if (i != -1) return;
                i++;
            }
            tt1.SetActive(IsEnter);
        }
    }
}
