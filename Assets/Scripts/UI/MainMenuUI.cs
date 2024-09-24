using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }
    [Header("Menu Screen")]
    [SerializeField] MenuButtonUI menuButtonUI;

    [Header("Data Slot Screen")]
    [SerializeField] DataSelectUI dataSelectUI;


    [Header("Character SelectUI Screen")]
    [SerializeField] CharacterSelectUI characterSelectUI;

    [Header("Settings Screen")]
    [SerializeField] SettingsUI settingsUI;

    [Header("Camera")]
    [SerializeField] Camera UI_Camera;
    [SerializeField] Camera world_Camera;
    [SerializeField] Canvas mainmenuCanvas;

    [SerializeField]UISelector[] UILayout;
    UISelector activeLayout;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UILayout = new UISelector[4] { menuButtonUI,dataSelectUI,characterSelectUI,settingsUI};

        SwitchUILayout(0);
    }

    private void Start()
    {
        
        AudioManager.Instance.PlayBGM("MainMenu");
    }
    public void SwitchUILayout(int index)
    {
        if(index < 0 || index >= UILayout.Length)
        {
            Debug.Log("invalid index");
            return;
        }
        if (index == 2)
        {
            SwitchCamera(false);
        }
        else { SwitchCamera(true); }

        foreach (var item in UILayout)
        {
            item.gameObject.SetActive(false);
        }

        activeLayout = UILayout[index];
        Debug.Log(UILayout[index]);
        if (UILayout[index].gameObject != null )
        {
            UILayout[index].gameObject.SetActive(true);
        }
       
    }
    public void SwitchCamera(bool isWorld)
    {
        UI_Camera.gameObject.SetActive(!isWorld);
        world_Camera.gameObject.SetActive(isWorld);
        
        if(isWorld)
        {
            mainmenuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        else
        {
            mainmenuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            mainmenuCanvas.worldCamera = UI_Camera;
        }
    }
    public void ExitGame()
    {
        Debug.Log("exit");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void OnSelectNewGameData()
    {
        SwitchUILayout(2);
    }
    public void OnSelectSavedGameData(GameData gameData)
    {
        SaveLoadManager.Instance.LoadAGame(gameData);
    }
}
