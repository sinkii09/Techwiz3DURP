using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class PickupTracking : Singleton<PickupTracking>,IDataPersistence
{
    public Dictionary<PowerUpType, Coroutine> activeCoroutines = new Dictionary<PowerUpType, Coroutine>(); 
    public Dictionary<PowerUpType, float> activeDurations = new Dictionary<PowerUpType, float>();
    public List<PickupItem> CollectedGems = new List<PickupItem>();
    public event Action<PowerUpType,bool> OnPowerUp;
    public event Action OnCoinPickup;
    public event Action<int> OnGemPickup;
    public event Action<bool> OntriggerDead;
    [SerializeField] int currentCoin;
    [SerializeField] int currentGemScore;
    public int CurrentGem => currentGemScore;

    public Dictionary<string,int> scoreBoard = new Dictionary<string, int>();
    #region PowerUp
    public void AddPowerUp(PowerUpItem item)
    {
        CollectedGems.Add(item);
        var data = item.PowerUpdata;
        if (activeCoroutines.ContainsKey(data.type))
        {
            activeDurations[data.type] = data.duration;
            if(HUD.Instance.keyValuePairs.ContainsKey(data.type))
            {
                HUD.Instance.keyValuePairs[data.type].Cooldown = data.duration;
            }
        }
        else
        {
            activeDurations[data.type] = data.duration;
            activeCoroutines[data.type] = StartCoroutine(HandleActivePowerUp(data.type));
        }
    }
    IEnumerator HandleActivePowerUp(PowerUpType type)
    {
        ApplyEffect(type);
        while (activeDurations[type] > 0)
        {
            yield return null;
            activeDurations[type] -= Time.deltaTime;
            HUD.Instance.keyValuePairs[type].Cooldown -= Time.deltaTime;
        }
        RemoveEffect(type);
        activeCoroutines.Remove(type);
        activeDurations.Remove(type);
    }
    void ApplyEffect(PowerUpType type)
    {
        Debug.Log("apply");
        OnPowerUp?.Invoke(type,true);
    }
    void RemoveEffect(PowerUpType type)
    {
        Debug.Log("remove");
        OnPowerUp?.Invoke(type,false);
    }
    #endregion

    #region Coin

    public void AddCoin()
    {
        currentCoin++;
        OnCoinPickup?.Invoke();
    }
    public void RemoveCoin(int amount)
    {
        currentCoin -= amount;
    }
    #endregion

    #region Gem
    public void AddGem(GemItem gemItem)
    {
        int gemPoint = GetGemValue(gemItem.GemType);
        CollectedGems.Add(gemItem);
        UpdateGemScoreValue(currentGemScore + gemPoint);
        OnGemPickup?.Invoke(gemPoint);
    }
    public void DescreaseGem()
    {
        if(currentGemScore <= 10)
        {
            UpdateGemScoreValue(0);
            OntriggerDead?.Invoke(true);
        }
        else
        {
            int amount = Mathf.RoundToInt(currentGemScore * .25f);
            int descreaseAmount = amount > 10 ? amount : 10;
            currentGemScore -= descreaseAmount;
            HUD.Instance.UpdateScoreText();
        }
    }
    int GetGemValue(GemType type)
    {
        switch (type)
        {
            case GemType.Red: return 100;
            case GemType.Blue: return 20;
            case GemType.Yellow: return 10;
            case GemType.Green: return 5;
            case GemType.White: return 1;
                default: return 0;
        }   
    }
    public void UpdateGemScoreValue(int value)
    {
        currentGemScore = value;
        HUD.Instance.UpdateScoreText();

    }
    public void Save(GameData data)
    {
        data.GemScore = currentGemScore;
    }

    public void Load(GameData data)
    {
        UpdateGemScoreValue(data.GemScore);
    }

    #endregion
}
