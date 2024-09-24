using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    [SerializeField] private List<Selectable> uiElements = new List<Selectable>();

    [SerializeField] int currentIndex = 0;

    private void OnEnable()
    {
        uiElements = gameObject.GetComponentsInChildren<Selectable>().ToList();
        if (uiElements.Count > 0)
        {
            SetSelect();
        }
    }
    private void OnDisable()
    {
        uiElements.Clear();
    }
    protected void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    MoveToNextElement();
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    MoveToPrevElement();
        //}
        //if(Input.GetKeyDown(KeyCode.J))
        //{
        //    if (uiElements[currentIndex].TryGetComponent(out Button button))
        //    {
        //        button.onClick.Invoke();
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    if(uiElements[currentIndex].TryGetComponent(out Slider slider))
        //    {
        //        slider.value += 0.05f;
        //    }
        //    if (uiElements[currentIndex].TryGetComponent(out Button button))
        //    {
        //        MoveToNextElement();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    if (uiElements[currentIndex].TryGetComponent(out Slider slider))
        //    {
        //        slider.value -= 0.05f;
        //    }
        //    if (uiElements[currentIndex].TryGetComponent(out Button button))
        //    {
        //        MoveToPrevElement();
        //    }
        //}
    }
    protected void MoveToNextElement()
    {
        currentIndex = (currentIndex + 1) % uiElements.Count;
        SetSelect();
    }
    protected void MoveToPrevElement()
    {
        int newIndex = Mathf.Max(0,currentIndex - 1);
        currentIndex = (newIndex)%uiElements.Count;
        SetSelect();
    }
    protected void SetSelect()
    {
        EventSystem.current.SetSelectedGameObject(uiElements[currentIndex].gameObject);
    }
}
