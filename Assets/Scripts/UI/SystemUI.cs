using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SystemUI : UISelector
{
    [Header("Systems Window")]
    [SerializeField] GameObject optionsPanel;
    [SerializeField] Button ResumeBtn;
    [SerializeField] Button LoadBtn;
    [SerializeField] Button ReturnMenuBtn;


    private void Start()
    {
        ResumeBtn.onClick.AddListener(() => { OnResumeBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        LoadBtn.onClick.AddListener(() => { OnLoadBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        ReturnMenuBtn.onClick.AddListener(() => { OnReturnMenuClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
    }
    private void OnDestroy()
    {
        ResumeBtn.onClick.RemoveAllListeners();
        LoadBtn.onClick.RemoveAllListeners();
        ReturnMenuBtn.onClick.RemoveAllListeners();
    }
    void OnResumeBtnClick()
    {
        GameManager.Instance.ResumeGame();
        optionsPanel.SetActive(false);
    }
    void OnLoadBtnClick()
    {
        PopupManager.Instance.ShowPopup(PopUpType.Notification, "Reload this game", OnLoadConfirm, null);
        
    }
    void OnLoadConfirm()
    {
        GameManager.Instance.ReloadGame();
    }
    void OnReturnMenuClick()
    {
        PopupManager.Instance.ShowPopup(PopUpType.Notification, "Are you sure want to exit", OnReturnMenuConfirm, null);
    }
    void OnReturnMenuConfirm()
    {
        GameManager.Instance.ReturnMenu();
    }
}
