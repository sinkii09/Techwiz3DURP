using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private const string HIGH_SCORE_FILE = "HighScores";


    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] bool useEncryption = true;
    [SerializeField] public GameData gameData;

    FileDataService fileDataService;
    List<IDataPersistence> dataPersistenceObjects;
    
    public SceneTransition SceneTransition { get { return sceneTransition; } }


    public HighScoreData HighScores;
    public int lastScore = 0;
    public override void Awake()
    {
        base.Awake();

        fileDataService = new FileDataService(new JsonSerializer(), useEncryption);
        HighScores = LoadHighScores();
        if (HighScores == null)
        {
            HighScores = new HighScoreData();
        }
    }
    private void Start()
    {

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

    }
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        Debug.Log("OnSceneLoad");
        if (scene.name.CompareTo("MainMenu") == 0) return;
        
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        OnLoad();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void NewGame(int index, string playerName)
    {
        Debug.Log("New Game");
        gameData = new GameData()
        {
            SlotIndex = index,
            Name = !string.IsNullOrEmpty(playerName) ? playerName : $"Player {index + 1}",
    };
    }
    public void SaveGame()
    {
        Debug.Log("Save Game");

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.Save(gameData);
        }
        gameData.SceneName = SceneManager.GetActiveScene().name;
        gameData.LastUpdated = DateTime.Now.ToBinary();
        fileDataService.Save(gameData,gameData.Name);
    }
    public void LoadAGame(GameData data) // both new and save game
    {
        gameData = data;
        sceneTransition.LoadSceneWithFade(gameData.SceneName);
    }
    void OnLoad()
    {
        Debug.Log("Load Game");

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.Load(gameData);
        }
    }
    public void DeleteData(GameData gameData)
    {
        fileDataService.Delete(gameData.Name);
        //NewGame(gameData.SlotIndex);
    }
    public GameData GetGameData(string gameName)
    {
        return fileDataService.Load<GameData>(gameName);
    }
    public IEnumerable<string> GetAllSaveDataName()
    {
        return fileDataService.ListSaves();
    }
    public GameData GetNewestSaveData()
    {
        return fileDataService.GetNewestsave();
    }
    public void SaveHighScores()
    {
        fileDataService.Save(HighScores, HIGH_SCORE_FILE);
    }

    public HighScoreData LoadHighScores()
    {
        return fileDataService.Load<HighScoreData>(HIGH_SCORE_FILE);
    }
}
