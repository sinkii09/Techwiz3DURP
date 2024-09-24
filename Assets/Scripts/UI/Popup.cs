using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType
{
    Notification,
    Tooltip,
}
public class Popup : UISelector
{
    [SerializeField] protected TextMeshProUGUI headerText;
    [SerializeField] protected TextMeshProUGUI messageText;
    [SerializeField] protected Button confirmButton;
    [SerializeField] protected Button cancelButton;
    PopupManager popupManager;
    public void Setup(PopupManager manager,PopUpType type, string message, Action onConfirm, Action onCancel)
    {
        popupManager = manager;
        headerText.text = type.ToString();
        messageText.text = message;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(()=>
        {
            AudioManager.Instance.PlaySFX("BtnClick");
            onConfirm?.Invoke();
            ClosePopup();
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX("BtnClick");
            onCancel?.Invoke(); 
            ClosePopup(); 
        });
    }
    protected void ClosePopup()
    {
        Destroy(gameObject);
    }
}
