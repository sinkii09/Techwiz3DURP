
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType
{ 
    White,
    Green,
    Yellow,
    Blue,
    Red,
    Orange,
}

public class GemItem : PickupItem
{
    

    //[ContextMenu("Generate guid for id")]
    //private void GenerateGuid()
    //{
    //    id = System.Guid.NewGuid().ToString();
    //}
    //private void Awake()
    //{
    //    id = gameObject.name;
    //}
    public GemType GemType;
    
    public int specialValue;
    

}
