using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFrameUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Button selectBtn;

    public void Initialize(CharacterSelectUI mainUI, CharacterData data)
    {
        iconImage.sprite = data.Icon;
        selectBtn.onClick.AddListener(() => { mainUI.SelectCharacter(data); AudioManager.Instance.PlaySFX("BtnClick"); });
    }
    private void OnDestroy()
    {
        selectBtn.onClick.RemoveAllListeners();
    }
}
