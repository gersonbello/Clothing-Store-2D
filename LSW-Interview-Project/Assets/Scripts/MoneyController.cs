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
    [Tooltip("Money Add effect UI text reference")]
    [SerializeField]
    private TextMeshProUGUI moneyAddText;
    [Tooltip("Text color when money add is positive")]
    [SerializeField]
    private Color moneyAddTextColorPositive;
    [Tooltip("Text color when money add is negative")]
    [SerializeField]
    private Color moneyAddTextColorNegative;
    // Reference to inicial position
    Vector2 moneyAddTextStartPosition;

    private void Start()
    {
        moneyAddTextStartPosition = moneyAddText.transform.position;
    }

    /// <summary>
    /// Change the money amount and start the courotine to animate the transiction
    /// </summary>
    /// <param name="value">The value of the change</param>
    public void AddMoney(int value)
    {
        StartCoroutine(ShowAddMoneyTextEffect(value));
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

    /// <summary>
    /// Show money add effect
    /// </summary>
    /// <param name="moneyValue">The value to show in money text</param>
    private IEnumerator ShowAddMoneyTextEffect(int moneyValue)
    {
        moneyAddText.transform.position = moneyAddTextStartPosition;
        string signText = moneyValue > 0 ? "+" : "-";
        moneyAddText.text = $"{signText} ${moneyValue}";
        moneyAddText.color = moneyValue > 0 ? moneyAddTextColorPositive : moneyAddTextColorNegative;

        while (moneyAddText.color.a > 0.01)
        {
            moneyAddText.transform.position += Vector3.up * 50 * Time.fixedDeltaTime;
            Color newColor = moneyAddText.color;
            newColor.a = Mathf.Lerp(moneyAddText.color.a, 0, .1f);
            moneyAddText.color = newColor;
            yield return null;
        }
        moneyAddText.color = new Color(0, 0, 0, 0);

    }
}
