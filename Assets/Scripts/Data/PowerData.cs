using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Power", menuName = " Power Data/Base Power")]
public class PowerData : ScriptableObject
{
    public enum PowerType
    {
        SpeedUp,
        Immortal,
        BonusPoint,
    }
    public PowerType type;
    public float duration;
    public bool isInstance;
    public Sprite icon;
    public GameObject vfx;
    public string description;
}
