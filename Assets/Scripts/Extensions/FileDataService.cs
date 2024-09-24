using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Linq;

public class FileDataService
{
    string filePath;
    string fileExtension;
    JsonSerializer serializer;
    bool useEncryption;

    public FileDataService(JsonSerializer serializer, bool useEncryption = true)
    {
        filePath = Application.persistentDataPath;
        fileExtension = "json";
        this.serializer = serializer;
        this.useEncryption = useEncryption;
    }
    string GetPathToFile(string fileName)
    {
        return Path.Combine(filePath, string.Concat(fileName, ".", fileExtension));
    }
    public void Save<T>(T data, string fileName, bool overwrite = true) where T : class
    {
        string fileLocation = GetPathToFile(fileName);
        if (!overwrite && File.Exists(fileLocation))
        {
            throw new IOException($"The file '{fileName}.{fileExtension}' already exists and cannot be overwritten.");
        }
        string saveData = serializer.Serialize(data);
        if (useEncryption)
        {
            saveData = DataEncryption.Encrypt(saveData);
        }
        File.WriteAllText(fileLocation, saveData);
    }

    public T Load<T>(string fileName) where T : class
    {
        string fileLocation = GetPathToFile(fileName);
        if (!File.Exists(fileLocation))
        {
            return null;
        }
        string loadData = File.ReadAllText(fileLocation);
        if (useEncryption)
        {
            loadData = DataEncryption.Decrypt(loadData);
        }
        return serializer.Deserialize<T>(loadData);
    }
    public void Delete(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
    }

    public void DeleteAll()
    {
        foreach (string filePath in Directory.GetFiles(filePath))
        {
            File.Delete(filePath);
        }
    }

    public IEnumerable<string> ListSaves()
    {
        foreach (string path in Directory.EnumerateFiles(filePath))
        {
            if ((Path.GetExtension(path) == "." + fileExtension) && Path.GetFileNameWithoutExtension(path) != "HighScores")
            {
                yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
    public GameData GetNewestsave()
    {
        var saveFiles = new List<GameData>();

        foreach (string fileName in ListSaves())
        {
            string fullPath = Path.Combine(filePath, fileName + "." + fileExtension);
            GameData gameData = Load<GameData>(fileName);

            if (gameData != null)
            {
                saveFiles.Add(new GameData()
                {
                    LastUpdated = gameData.LastUpdated,
                });
            }
        }

        return saveFiles.OrderByDescending(s => s.LastUpdated).FirstOrDefault();
    }
}