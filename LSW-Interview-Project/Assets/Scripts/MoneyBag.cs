using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : InteractableObject
{
    /// <summary>
    /// Simple public method for events
    /// </summary>
    /// <param name="value"></param>
    public void AddMoney(int value)
    {
        GameController.gcInstance.moneyController.AddMoney(value);
    }
}
