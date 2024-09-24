using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float loadDelay;

    [SerializeField] SceneTransition transition;
    void Start()
    {
        StartCoroutine(LoadMenuDelay());
    }
    IEnumerator LoadMenuDelay()
    {
        while(loadDelay > 0)
        {
            yield return null;
            loadDelay -= Time.deltaTime;
        }
        transition.LoadSceneWithFade("MainMenu");

    }
}
