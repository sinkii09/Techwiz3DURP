using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button loadBtn;
    [SerializeField] Button returnBtn;

    private void Start()
    {
        loadBtn.onClick.AddListener(() => { OnLoadBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        returnBtn.onClick.AddListener(() => { OnReturnBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
    }

    private void OnDestroy()
    {
        loadBtn.onClick.RemoveAllListeners();
        returnBtn.onClick.RemoveAllListeners();
    }
    private void OnLoadBtnClick()
    {
        GameManager.Instance.ReloadGame();
    }

    private void OnReturnBtnClick()
    {
        GameManager.Instance.ReturnMenu();
    }

    public void Show()
    {
        scoreText.text = $"Score: {PickupTracking.Instance.CurrentGem}";
        gameObject.SetActive(true);
    }
}
