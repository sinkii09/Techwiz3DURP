using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerTracking : MonoBehaviour
{
    [SerializeField] int maxConsume = 200;
    [SerializeField] int currentTrack;

    PowerData powerData;
    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
    }
    private void Start()
    {
        Debug.Log(playerController.CharacterData.power.type);
        powerData = playerController.CharacterData.power;
        CanUsePower = false;
    }
    public bool CanUsePower { get; private set; }

    private bool isUsingPower;
    private void Update()
    {
        UpdateTracking();
        HUD.Instance.UpdateSliderValue((float)currentTrack/maxConsume);
    }

    public void TrackingPower(int point)
    {
        if (isUsingPower) return;
        if (currentTrack == maxConsume) return;
        currentTrack += point;
    }
    void UpdateTracking()
    {
        if (powerData == null) return;
        if (isUsingPower) return;
        if (currentTrack < maxConsume) return;
        CanUsePower = true;
        if (powerData.isInstance)
        {
            ActivatePower();
        }
    }
    public void ActivatePower()
    {
        if (!CanUsePower) return;
        Debug.Log("activate");
        CanUsePower = false;
        isUsingPower = true;
        StartCoroutine(UsingPower(powerData.duration));
    }
    public IEnumerator UsingPower(float duration)
    {
        playerController.ApplyPowerEffect();
        int trackVelocity = Mathf.RoundToInt(currentTrack/(duration*10));
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.1f);
            duration -= 0.1f;
            currentTrack -= trackVelocity;
        }
        playerController.RemovePowerEffect();
        ResetTracking();
    }
    public void ResetTracking()
    {
        Debug.Log("reset track");
        currentTrack = 0;
        isUsingPower = false;
    }
}
