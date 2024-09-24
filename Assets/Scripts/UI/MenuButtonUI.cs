using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MenuButtonUI : UISelector
{
    [SerializeField] Button startBtn;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button quitBtn;

    [Header("rank")]
    [SerializeField] PlayerRecordUI playerRecordUIPrefab;
    [SerializeField] Transform recordholder;
    List<PlayerRecordUI> recordList = new List<PlayerRecordUI>();

    Dictionary<string, int> ScoreDict = new Dictionary<string, int>();
    private void Start()
    {
        startBtn.onClick.AddListener(() => { OnStartBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        settingsBtn.onClick.AddListener(() => { OnSettingsBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        quitBtn.onClick.AddListener(() => { OnQuitBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        CalculateScoreBoard();
    }
    private void OnEnable()
    {
        
    }
    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
        settingsBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();

    }
    private void OnStartBtnClick()
    {
        MainMenuUI.Instance.SwitchUILayout(1);
    }

    void OnSettingsBtnClick()
    {
        MainMenuUI.Instance.SwitchUILayout(3);
    }
    void OnQuitBtnClick()
    {
        PopupManager.Instance.ShowPopup(PopUpType.Notification, "Are you sure want to Quit", MainMenuUI.Instance.ExitGame, null);
    }

    void CalculateScoreBoard()
    {
        var highScores = SaveLoadManager.Instance.HighScores.highScores;

        if(highScores == null ) { return; }

        if (recordList.Count > 0)
        {
            foreach(var child  in recordList)
            {
                Destroy(child);
            }
            recordList.Clear();
        }
        for (int i = 0; i < Math.Min(5, highScores.Count); i++)
        {
            var record = Instantiate(playerRecordUIPrefab, recordholder);
            record.Setup(highScores[i].playerName, highScores[i].score.ToString());
            recordList.Add(record);
        }
    }
}
