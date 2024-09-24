using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : PickupItem
{
    public PowerUpdata PowerUpdata;
    private void Start()
    {
        Instantiate(PowerUpdata.visual,transform);
    }
}
