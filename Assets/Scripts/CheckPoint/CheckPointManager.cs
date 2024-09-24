using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointManager : MonoBehaviour,IDataPersistence
{
    [SerializeField] CheckPoint[] checkPoints;

    CheckPoint lastCheckPoint;
    private void Awake()
    {
        checkPoints = GetComponentsInChildren<CheckPoint>();
    }
    private void Start()
    {
        EventManager.Addlistener("CheckPoint", OnCheckPointTrigger);
    }
    private void OnDestroy()
    {
        EventManager.Removelistener("CheckPoint", OnCheckPointTrigger);
    }
    private void OnCheckPointTrigger(object obj)
    {
        var checkPoint = (CheckPoint)obj;
        //foreach(var child in checkPoints)
        //{
        //    if(child != null && child != checkPoint)
        //    {
        //        child.DisableEffect();
        //    }
        //}
        lastCheckPoint = checkPoint;
        Debug.Log("check point save");
        GameManager.Instance.SaveGame();
    }

    public void Save(GameData data)
    {
        if(lastCheckPoint!=null)
        {
            data.CheckPointId = lastCheckPoint.Id;
        }
    }

    public void Load(GameData data)
    {
        if (data.CheckPointId == null) return;
        lastCheckPoint = checkPoints.Where(x => x.Id == data.CheckPointId).FirstOrDefault();
        if(lastCheckPoint == null) return;
        lastCheckPoint.EnableFX();
    }
}
