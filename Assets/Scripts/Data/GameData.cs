using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameData
{
    public string Name;
    public int SlotIndex;
    public long LastUpdated;
    public string SceneName;
    public Vector3 LastPositionPos;
    public CharacterType CharacterType;
    public SerializableDictionary<string,bool> CollectedPickup;
    public SerializableDictionary<string,bool> CheckedPoints;
    public SerializableDictionary<int,float> DurationTime;
    public int GemScore;
    public bool isSaved;
    public string CheckPointId;
    public string WaypointID;
    public GameData() 
    {
        //Name = !string.IsNullOrEmpty(name) ? name : $"Player {SlotIndex + 1}" ;
        LastUpdated = 0;
        SceneName = "map1";
        LastPositionPos = Vector3.zero;
        CharacterType = CharacterType.Warrior;
        CollectedPickup = new SerializableDictionary<string, bool>();
        CheckedPoints = new SerializableDictionary<string, bool>();
        DurationTime = new SerializableDictionary<int,float>();
        GemScore = 0;
        isSaved = false;
    }

}
[System.Serializable]
public class HighScoreData
{
    public List<PlayerScore> highScores = new List<PlayerScore>();
}
[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}
