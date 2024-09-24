using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UISelector
{
    [SerializeField] GameObject settingsScreen;
    [SerializeField] Slider bgm_Slider;
    [SerializeField] Slider sfx_Slider;
    [SerializeField] Button confirmBtn;

    private void Start()
    {
        bgm_Slider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfx_Slider.onValueChanged.AddListener(OnSfxSliderChanged);
        confirmBtn.onClick.AddListener(()=> { HideSetting(); AudioManager.Instance.PlaySFX("BtnClick"); });
    }

    private void OnDestroy()
    {
        bgm_Slider.onValueChanged.RemoveAllListeners();
        sfx_Slider.onValueChanged.RemoveAllListeners();
        confirmBtn.onClick.RemoveAllListeners();
    }
    private void OnEnable()
    {
        bgm_Slider.value = AudioManager.Instance.GetBGMVolume();
        sfx_Slider.value = AudioManager.Instance.GetSFXVolume();
    }
    public void HideSetting() 
    {
        MainMenuUI.Instance.SwitchUILayout(0);
    }

    private void OnBgmSliderChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);

    }

}
