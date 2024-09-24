using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : Singleton<PopupManager>
{

    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private GameObject inputPopupPrefab;
    [SerializeField] private GameObject timerPopupPrefab;


    CanvasGroup popupCanvasGroup;
    [SerializeField] List<CanvasGroup> uiCanvasGroups = new List<CanvasGroup>();
    private void Start()
    {
        popupCanvasGroup = GetComponent<CanvasGroup>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        uiCanvasGroups.Clear();
        uiCanvasGroups = FindObjectsOfType<CanvasGroup>().ToList();
    }

    public void ShowPopup(PopUpType type,string message, Action onConfirm, Action onCancel)
    {

        GameObject popupInstance = Instantiate(popupPrefab, transform);

        Popup popupScript = popupInstance.GetComponent<Popup>();
        popupScript.Setup(this,type,message, onConfirm, onCancel);
    }
    public void ShowInputPopup(bool isOverwrite,PopUpType type, string message, Action<string,bool> onConfirm, Action onCancel)
    {
        GameObject popupInstance = Instantiate(inputPopupPrefab, transform);
        InputPopup popupScript = popupInstance.GetComponent<InputPopup>();
        popupScript.SetupInput(isOverwrite, type, message, onConfirm, onCancel);
    }
    #region timer popup
    public void ShowTimedPopup(string header, string message, Action onConfirm, Action onCancel, float duration)
    {
        GameObject popup = Instantiate(timerPopupPrefab, transform);
        StartCoroutine(CloseAfterDelay(popup, duration));
    }

    private IEnumerator CloseAfterDelay(GameObject popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(popup != null)
        {
            Destroy(popup);
        }
    }
    #endregion
}
