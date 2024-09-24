using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : Singleton<HUD>
{
    [SerializeField] Slider powerUpSlider;
    [SerializeField] float lerpBuffer = 5f;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;

    private int score = 0;
    private Coroutine scoreLerpCoroutine;

    [SerializeField] Button ShowSystemUIBtn;
    [SerializeField] UISelector systemWindow;
    [SerializeField] Slider bgm_Slider;
    [SerializeField] Slider sfx_Slider;

    [SerializeField] GameOverUI gameOverUI;

    [SerializeField] PowerStatusUI powerStatusUIPrefab;
    [SerializeField] Transform statusContainer;
    public Dictionary<PowerUpType, PowerStatusUI> keyValuePairs = new Dictionary<PowerUpType, PowerStatusUI>();
    public override void Awake()
    {
        base.Awake();
        systemWindow.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);

    }
    private void Start()
    {
        ShowSystemUIBtn.onClick.AddListener(() => { OnShowSystemsBtnClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        bgm_Slider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfx_Slider.onValueChanged.AddListener(OnSfxSliderChanged);
        PickupTracking.Instance.OnPowerUp += Instance_OnPowerUp;
    }
    private void OnEnable()
    {
        bgm_Slider.value = AudioManager.Instance.GetBGMVolume();
        sfx_Slider.value = AudioManager.Instance.GetSFXVolume();
    }
    private void OnDestroy()
    {
        bgm_Slider.onValueChanged.RemoveAllListeners();
        sfx_Slider.onValueChanged.RemoveAllListeners();
        ShowSystemUIBtn.onClick.RemoveAllListeners();
    }
    private void OnBgmSliderChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);

    }
    private void Instance_OnPowerUp(PowerUpType type, bool isActive)
    {
        var powerupData = CharacterDataBase.Instance.GetPowerUpData(type);
        if (powerupData == null) return;

        if(isActive)
        {
            if(!keyValuePairs.ContainsKey(type) || keyValuePairs[type] == null) 
            {
                keyValuePairs[type] = Instantiate(powerStatusUIPrefab, statusContainer);
                keyValuePairs[type].Setup(powerupData);
            }
        }
        else
        {
            Debug.Log("isactive" + isActive);
            Debug.Log(keyValuePairs.ContainsKey(type));
            if(keyValuePairs.ContainsKey(type))
            {
                Destroy(keyValuePairs[type].gameObject);
                keyValuePairs.Remove(type);
            }
        }
    }

    public void UpdateSliderValue(float value)
    {
        powerUpSlider.value = Mathf.LerpUnclamped(powerUpSlider.value, value, Time.deltaTime * lerpBuffer);
    }
    public void UpdateScoreText()
    {
        if (scoreLerpCoroutine != null)
        {
            StopCoroutine(scoreLerpCoroutine);
        }
        scoreLerpCoroutine = StartCoroutine(LerpScoreText());
    }
    private IEnumerator LerpScoreText()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        int startScore = score;
        int targetScore = PickupTracking.Instance.CurrentGem;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            score = Mathf.RoundToInt(Mathf.LerpUnclamped(startScore, targetScore, elapsedTime / duration));
            scoreText.text = $"{score}";

            yield return null;
        }
        score = targetScore;
        scoreText.text = $"{score}";
    }
    public void UpdateCountDownText(TimeSpan timeSpan)
    {
        timerText.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
    }

    void OnShowSystemsBtnClick()
    {
        GameManager.Instance.PauseGame();
        systemWindow.gameObject.SetActive(true);
    }
    public void ShowGameOver()
    {
        gameOverUI.Show();
    }
}
