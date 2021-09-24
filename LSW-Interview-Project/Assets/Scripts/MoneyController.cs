using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    [Header("Money UI Configuration")]
    [Tooltip("Player money amount")]
    [SerializeField]
    private int moneyAcount;
    [Tooltip("Money UI text reference")]
    [SerializeField]
    private TextMeshProUGUI moneyText;

    /// <summary>
    /// Change the money amount and start the courotine to animate the transiction
    /// </summary>
    /// <param name="value">The value of the change</param>
    public void AddMoney(int value)
    {
        StartCoroutine(SetMoneyTo(moneyAcount + value));
    }

    /// <summary>
    /// Change money value to the new over time
    /// </summary>
    /// <param name="newValue"></param>
    /// <returns></returns>
    private IEnumerator SetMoneyTo(int newValue)
    {
        float oldMoney = moneyAcount;
        moneyAcount = newValue;
        // A timer to force the value to be changed to the end result in the same time in any case
        float basicTimer = 0;

        while (basicTimer < 1)
        {
            basicTimer += Time.deltaTime;

            // Get a dynamic speed to change the money value in the same time
            float moneyChangeSpeed = ((newValue - oldMoney) / (1 / Time.deltaTime));

            oldMoney += moneyChangeSpeed;
            SetMoneyText((int)oldMoney);

            yield return null;
        }
        SetMoneyText(moneyAcount);
    }
    /// <summary>
    /// Change the current UI money text
    /// </summary>
    /// <param name="moneyValue">The value to show in money text</param>
    private void SetMoneyText(int moneyValue)
    {
        moneyText.text = $"${moneyValue}";
    }
}
