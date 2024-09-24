using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterDataBase : Singleton<CharacterDataBase>
{
    public static CharacterDataBase instance;
    [SerializeField] CharacterData[] CharacterData;
    public CharacterData[] CharacterDataArray => CharacterData;

    public void PickupCharacter(CharacterType type)
    {
        SaveLoadManager.Instance.gameData.CharacterType = type;
    }
    public CharacterData GetPickupCharacter(CharacterType type)
    {
       return CharacterData.Where(x => x.characterType == type).FirstOrDefault();
    }

    [Header("Power Icons")]
    [SerializeField] PowerUpdata freezeTimedata;
    [SerializeField] PowerUpdata doubleJumpdata;
    public Dictionary<PowerUpType, Sprite> powerUpSprites;

    public PowerUpdata GetPowerUpData(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.FrozenTime:
                return freezeTimedata;
            case PowerUpType.DoubleJump:
                return doubleJumpdata;
            default: return null;
        }
    }
}
