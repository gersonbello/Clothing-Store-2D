using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour
{
    // Player money amount
    public int moneyAcount { get; private set; }

    [Header("Money UI Configuration")]
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
        moneyAddText.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// Change the money amount and start the courotine to animate the transiction
    /// </summary>
    /// <param name="value">The value of the change</param>
    public void AddMoney(int value)
    {
        StopAllCoroutines();
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

        while (Mathf.Abs(oldMoney - newValue) > 1)
        {
            oldMoney = Mathf.Lerp(oldMoney, newValue, .1f);
            SetMoneyText((int)oldMoney);

            yield return null;
        }
        SetMoneyText(moneyAcount);
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

    /// <summary>
    /// Change the current UI money text
    /// </summary>
    /// <param name="moneyValue">The value to show in money text</param>
    private void SetMoneyText(int moneyValue)
    {
        moneyText.text = $"${moneyValue}";
    }

}
