using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Warrior,
    Drawf,
    Alchemist,
}
[CreateAssetMenu(fileName = "new Character", menuName = " Character Data")]
public class CharacterData : ScriptableObject
{
    public CharacterType characterType; 
    public PowerData power;
    public GameObject Visual;
    public Sprite Icon;
}
