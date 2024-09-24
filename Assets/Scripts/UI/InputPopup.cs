using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputPopup : Popup
{
    [SerializeField] TMP_InputField inputField;
    public void SetupInput(bool isOverwrite,PopUpType type, string message, Action<string,bool> onConfirm, Action onCancel)
    {
        headerText.text = type.ToString();
        messageText.text = message;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX("BtnClick");
            onConfirm?.Invoke(inputField.text,isOverwrite);
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
}
