using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button returnBtn;

    private void Start()
    {
        AudioManager.Instance.PlayBGM("End");
        if(SaveLoadManager.Instance.gameData != null)
        {
            scoreText.text = SaveLoadManager.Instance.lastScore.ToString();
        }
        returnBtn.onClick.AddListener(() => { SceneManager.LoadScene("MainMenu"); AudioManager.Instance.PlaySFX("BtnClick"); });
    }
    private void OnDestroy()
    {
        returnBtn.onClick.RemoveAllListeners();
    }
}
