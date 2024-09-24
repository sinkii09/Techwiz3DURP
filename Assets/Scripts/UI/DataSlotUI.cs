using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSlotUI : MonoBehaviour
{

    [Header("Header")]
    [SerializeField] TextMeshProUGUI headerText;

    [Header("Content")]
    [SerializeField] GameObject newContent;
    [SerializeField] GameObject loadContent;
    [SerializeField] Button slotBtn;
    [SerializeField] Image mapImage;
    [SerializeField] TextMeshProUGUI mapNameText;
    [SerializeField] TextMeshProUGUI dateText;

    [Header("Footer")]
    [SerializeField] GameObject footerObj;
    [SerializeField] Button loadBtn;
    [SerializeField] Button ovewriteBtn;

    GameData gameData;
    public int slot { get; set; }
    private void Awake()
    {
        loadContent.SetActive(false);
        footerObj.SetActive(false);
    }
    private void Start()
    {
        slotBtn.onClick.AddListener(() => { OnSlotBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        loadBtn.onClick.AddListener(() => { OnLoadBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        ovewriteBtn.onClick.AddListener(() => { OnOverwriteBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });

    }
    private void OnEnable()
    {
        if(gameData!=null)
        {
            loadContent.SetActive(true);
        }
    }
    private void OnDisable()
    {
        footerObj.SetActive(false);
    }
    public void Initialize(GameData data)
    {
        gameData = data;
        mapNameText.text = data.Name;


        DateTime lastUpdated = DateTime.FromBinary(gameData.LastUpdated);
        dateText.text = lastUpdated.ToString();

        newContent.SetActive(false);
        loadContent.SetActive(true);
    }

    private void OnLoadBtnClick()
    {
        MainMenuUI.Instance.OnSelectSavedGameData(gameData);
    }

    private void OnOverwriteBtnClick()
    {
        PopupManager.Instance.ShowPopup(PopUpType.Notification, "Are you sure want to overwrite this save",ConfirmOverWrite, null);
    }
    private void ConfirmOverWrite()
    {
        OpenInputName(true);
    }

    private void OnSlotBtnClick()
    {
        if(gameData!=null)
        {
            footerObj.SetActive(true);
        }
        else
        {
            OpenInputName(false);
        }
    }

    internal void SetHeaderText(int i)
    {
        headerText.text = (i+1).ToString();
    }
    void OpenInputName(bool isOverwrite)
    {
        PopupManager.Instance.ShowInputPopup(isOverwrite,PopUpType.Notification, "Input Name", CreateNewGame, null);
    }
    void CreateNewGame(string playerName,bool isOverwrite)
    {
        if(isOverwrite)
        {
            SaveLoadManager.Instance.DeleteData(gameData);
        }
        SaveLoadManager.Instance.NewGame(slot,playerName);
        MainMenuUI.Instance.OnSelectNewGameData();
    }
}
