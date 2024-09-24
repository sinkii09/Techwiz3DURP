using ithappy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour, IDataPersistence
{
    [field:SerializeField] public string Id;
    public bool isChecked;

    [SerializeField] OscillatePosition oscillatePosition;
    [SerializeField] GameObject vfx;
    private void Awake()
    {
        DisableEffect();
    }
    public void DisableEffect()
    {
        oscillatePosition.enabled =false;
        vfx.SetActive(false);
    }
    
    public void Trigger()
    {
        Debug.Log(Id);
        isChecked = true;
        EnableFX();
        EventManager.TriggerEvent("CheckPoint", this);
    }
    public void EnableFX()
    {
        oscillatePosition.enabled = true;
        vfx.SetActive(true);
    }
    public void Save(GameData data)
    {
        if (data.CheckedPoints.ContainsKey(Id))
        {
            data.CheckedPoints.Remove(Id);
        }
        data.CheckedPoints[Id] = isChecked;
    }
    public void Load(GameData data)
    {
        if(data.CheckedPoints.ContainsKey(Id))
        {
            isChecked = data.CheckedPoints[Id];
            if(isChecked)
            {
                EnableFX();
            }
        }

    }
}
