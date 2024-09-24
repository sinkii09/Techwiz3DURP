using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataSelectUI : UISelector
{
    [SerializeField] DataSlotUI[] DataSlots;
    [SerializeField] Button backToMenuBtn;

    private void Start()
    {
        backToMenuBtn.onClick.AddListener(() => { OnBackBtnClick(); });
    }
    private void OnEnable()
    {
        Show();
    }
    private void OnDestroy()
    {
        backToMenuBtn.onClick.RemoveAllListeners();
    }
    public void Show()
    {
        for (int i = 0; i < DataSlots.Length; i++)
        {
            DataSlots[i].slot = i;
            DataSlots[i].SetHeaderText(i);
        }
        var dataList = SaveLoadManager.Instance.GetAllSaveDataName().ToList();

        for (int i = 0; i < dataList.Count; i++)
        {
            GameData data = SaveLoadManager.Instance.GetGameData(dataList[i]);
            DataSlots[data.SlotIndex].Initialize(data);
        }

        gameObject.SetActive(true);
    }

    private void OnBackBtnClick()
    {
        AudioManager.Instance.PlaySFX("BtnClick");
        MainMenuUI.Instance.SwitchUILayout(0);
    }

}
