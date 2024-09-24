using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>,IDataPersistence
{
    public bool IsGameStart {  get; set; }
    public bool IsGameOver { get; set; }
    public bool FreezeCountdown { get; set; }

    [SerializeField] float countdownTimer;

    private float timeDuration;

    public override void Awake()
    {
        ResumeGame();
    }
    private void Start()
    {
        IsGameStart = true;
        PlaySceneMusic();

    }
    private void FixedUpdate()
    {
        CountDown();
    }
    void CountDown()
    {
        if (!IsGameStart) return;
        if(timeDuration <= 0)
        {
            timeDuration = 0;
            GameOver();
        }
        if(!FreezeCountdown)
        {
            timeDuration -= Time.fixedDeltaTime;
        }
        TimeSpan span = TimeSpan.FromSeconds(timeDuration);
        HUD.Instance.UpdateCountDownText(span);
    }
    public void GameOver()
    {
        if (!IsGameOver)
        {
            PauseGame();
            IsGameOver = true;
            HUD.Instance.ShowGameOver();
            SaveGame();
            AddHighScore();
        }
    }
    
    public void PauseGame() => Time.timeScale = 0;
    public void ResumeGame() => Time.timeScale = 1;
    public void SaveGame()
    {
        SaveLoadManager.Instance.SaveGame();
    }
    public void ReloadGame() // load newest save of current data
    {
        ResumeGame();
        SaveLoadManager.Instance.LoadAGame(SaveLoadManager.Instance.gameData);
    }

    internal void ToNextLevel()
    {
        if(SceneManager.GetActiveScene().name == "Map2")
        {
            SaveLoadManager.Instance.lastScore = PickupTracking.Instance.CurrentGem;
            AddHighScore();
        }
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        ResumeGame();
        var data = SaveLoadManager.Instance.gameData;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        string scenePath = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);

        if (!string.IsNullOrEmpty(scenePath))
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            data.SceneName = sceneName;
            Debug.Log($"Next scene name: {data.SceneName}");
            Debug.Log($"Next scene index: {nextSceneIndex}");

            data.isSaved = false;
            SaveLoadManager.Instance.LoadAGame(data);
        }
        else
        {
            Debug.LogError($"No scene found at build index {nextSceneIndex}");
        }
    }
    void PlaySceneMusic()
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        string scenePath = SceneUtility.GetScenePathByBuildIndex(SceneIndex);
        if (!string.IsNullOrEmpty(scenePath))
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if(sceneName == "map1")
            {
                AudioManager.Instance.PlayBGM("Battle");
            }
            else if(sceneName == "Map2")
            {
                AudioManager.Instance.PlayBGM("Dungeon");
            }
        }
    }
    internal void ReturnMenu()
    {
        ResumeGame();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Save(GameData data)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.DurationTime[sceneIndex] = timeDuration;
    }

    public void Load(GameData data)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (data.DurationTime.ContainsKey(sceneIndex))
        {
            timeDuration = data.DurationTime[sceneIndex];
        }
        else
        {
            timeDuration = countdownTimer;
        }
    }
    public void AddHighScore()
    {
        var gameData = SaveLoadManager.Instance.gameData;
        var highScores = SaveLoadManager.Instance.HighScores.highScores;
        highScores.Add(new PlayerScore(gameData.Name, PickupTracking.Instance.CurrentGem));
        highScores = highScores.OrderByDescending(s => s.score).ToList();

        int maxScores = 10;
        if (highScores.Count > maxScores)
        {
            highScores = highScores.Take(maxScores).ToList();
        }
        SaveLoadManager.Instance.HighScores.highScores = highScores;
        SaveLoadManager.Instance.SaveHighScores();
    }
}
