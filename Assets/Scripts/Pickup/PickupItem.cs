using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public void Pickup();
}
public class PickupItem : MonoBehaviour, IPickupable,  IDataPersistence
{
    [field: SerializeField] public string id;
    public enum PickupType
    { 
        Key,
        PowerUp,
        Gem,
    }
    [SerializeField] protected PickupType type;
    public GameObject pickupVFX;
    public PickupType Type => type;

    public bool isCollected = false;
    public void Save(GameData data)
    {
        if (type == PickupType.Key) { return; }
        if (data.CollectedPickup.ContainsKey(id))
        {
            data.CollectedPickup.Remove(id);
        }
        data.CollectedPickup[id] = isCollected;
    }
    public void Load(GameData data)
    {
        if (type == PickupType.Key) { return; }
        if(data.CollectedPickup.ContainsKey(id))
        {
            isCollected = data.CollectedPickup[id];
        }
        if (isCollected)
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void Pickup()
    {
        gameObject.SetActive(false);
        isCollected = true;
    }
}
