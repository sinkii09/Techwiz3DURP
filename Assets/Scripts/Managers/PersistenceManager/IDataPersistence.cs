using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence 
{
    void Save(GameData data);
    public void Load(GameData data);
}
