using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : UISelector
{
    [SerializeField] CharacterFrameUI characterFramePrefab;

    [Header("1st Layout")]
    [SerializeField] RectTransform characterIconContainer;
    [SerializeField] RectTransform characterVisualContainer;
    [SerializeField] RectTransform characterInfoContainer;

    [Header("2nd Layout")]
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI PowerNameText;
    [SerializeField] TextMeshProUGUI PowerDescriptionText;
    [SerializeField] Image PowerIcon;

    [Header("Btn")]
    [SerializeField] Button closeBtn;
    [SerializeField] Button playBtn;

    List<CharacterData> charList = new List<CharacterData>();
    CharacterData currentSelectCharData;
    GameObject currentCharVisual;

    private void Start()
    {
        closeBtn.onClick.AddListener(()=> {
            MainMenuUI.Instance.SwitchUILayout(1);
        });
        playBtn.onClick.AddListener(()=> { OnPlayGameClick(); AudioManager.Instance.PlaySFX("BtnClick"); });
        charList = CharacterDataBase.Instance.CharacterDataArray.ToList();

        foreach (CharacterData charData in charList)
        {
            var iconHolder = Instantiate(characterFramePrefab, characterIconContainer);
            iconHolder.Initialize(this,charData);
        }
    }
    private void OnEnable()
    {
        characterInfoContainer.GetChild(0).gameObject.SetActive(false);
        playBtn.interactable = true;
    }
    private void OnDestroy()
    {
        closeBtn.onClick.RemoveAllListeners();
        playBtn.onClick.RemoveAllListeners();
    }
    public void SelectCharacter(CharacterData charData)
    {
        if (currentSelectCharData == charData) return;
        currentSelectCharData = charData;
        
        if(currentCharVisual != null)
        {
            Destroy(currentCharVisual);
        }
        currentCharVisual = Instantiate(currentSelectCharData.Visual, characterVisualContainer.GetChild(0));

        if(characterInfoContainer.GetChild(0).gameObject.activeSelf == false)
        {
            characterInfoContainer.GetChild(0).gameObject.SetActive(true);
        }

        characterNameText.text = currentSelectCharData.characterType.ToString();
        PowerNameText.text = currentSelectCharData.power.type.ToString();
        PowerDescriptionText.text = currentSelectCharData.power.description.ToString();
        if(!PowerIcon.enabled)
        {
            PowerIcon.enabled = true;
        }
        PowerIcon.sprite = currentSelectCharData.power.icon;
        
    }
    private void OnPlayGameClick()
    {
        if (currentSelectCharData == null)
        {
            PopupManager.Instance.ShowPopup(PopUpType.Notification, "Please select a character", null, null);
            return;
        }
        playBtn.interactable = false;

        CharacterDataBase.Instance.PickupCharacter(currentSelectCharData.characterType);
        
        SaveLoadManager.Instance.LoadAGame(SaveLoadManager.Instance.gameData);
    }
}
