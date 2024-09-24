using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    FrozenTime,
    DoubleJump,
    SpeedUp,
}
[CreateAssetMenu(fileName = "new PowerUp", menuName = " Power Data/PowerUp")]
public class PowerUpdata : ScriptableObject
{

    public PowerUpType type;
    public float duration;
    public GameObject visual;
    public Sprite icon;
}
