using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum StoreSection
{
    Hats,
    Bodys,
    Hands,
    Feets
}
public class SkinShop : MonoBehaviour
{

    [Header("Skin Store Configuration")]
    [Tooltip("Selected Skin Index")]
    [SerializeField]
    private int skinIndex;
    [Tooltip("Selected Skin")]
    [SerializeField]
    private Skin selectedSkin;
    [Tooltip("Reference to the UI renderer of the skin showcase")]
    [SerializeField]
    private Image skinShowcaseRenderer;
    [Tooltip("Setted direction of character representation")]
    [SerializeField]
    private Direction settedDirection = Direction.right;
    [Tooltip("Setted direction of character representation")]
    [SerializeField]
    private StoreSection settedSection = StoreSection.Hats;
    [Tooltip("Character representation reference")]
    [SerializeField]
    private MoveableObjects characterRepresentation;

    [Header("Text References")]
    [Tooltip("Buy Button Text reference")]
    [SerializeField]
    private TextMeshProUGUI buyButtonText;
    [Tooltip("Section Text reference")]
    [SerializeField]
    private TextMeshProUGUI sectionText;
    [Tooltip("Skin Name Text reference")]
    [SerializeField]
    private TextMeshProUGUI skinNameText;
    [Tooltip("Price Text reference")]
    [SerializeField]
    private TextMeshProUGUI priceText;

    [Header("Buttons References")]
    [Tooltip("Buy Button reference")]
    [SerializeField]
    private Button buyButton;

    [Header("Skins References")]
    [Tooltip("Hat Skins")]
    [SerializeField]
    private List<Skin> hats;
    [Tooltip("Body Skins")]
    [SerializeField]
    private List<Skin> bodys;
    [Tooltip("Hand Skins")]
    [SerializeField]
    private List<Skin> hands;
    [Tooltip("Feet Skins")]
    [SerializeField]
    private List<Skin> feets;

    private void Awake()
    {
        ResetSkins(hats);
        ResetSkins(hands);
        ResetSkins(bodys);
        ResetSkins(feets);
        ShowSkinSection();
    }

    /// <summary>
    /// Reset the scriptables to default value, coud be used to save and load values too
    /// </summary>
    /// <param name="skinsList">List of skins to be reseted</param>
    private void ResetSkins(List<Skin> skinsList)
    {
        foreach (Skin s in skinsList) s.bought = false;
    }

    /// <summary>
    /// Rotate to next direction
    /// </summary>
    public void NextDirection()
    {
        settedDirection = (Direction)Mathf.Repeat(settedDirection.GetHashCode() + 1, 4);
        characterRepresentation.ChangeSkinLookingAtDirection(settedDirection);
    }
    /// <summary>
    /// Rotate to previous direction
    /// </summary>
    public void PreviousDirection()
    {
        settedDirection = (Direction)Mathf.Repeat(settedDirection.GetHashCode() - 1, 4);
        characterRepresentation.ChangeSkinLookingAtDirection(settedDirection);
    }

    /// <summary>
    /// Show the next skin in the list
    /// </summary>
    public void ShowNext()
    {
        skinIndex++;
        ShowSkinSection();
    }
    /// <summary>
    /// Show the previous skin in the list
    /// </summary>
    public void ShowPrevious()
    {
        skinIndex--;
        ShowSkinSection();
    }

    /// <summary>
    /// Show skin by the selected section
    /// </summary>
    private void ShowSkinSection()
    {
        switch (settedSection)
        {
            case StoreSection.Hats:
                sectionText.text = "hats";
                skinIndex = skinIndex >= hats.Count ? 0 : skinIndex;
                skinIndex = skinIndex < 0 ? hats.Count - 1 : skinIndex;
                ShowSkinOnShop(hats[skinIndex]);
                break;
            case StoreSection.Bodys:
                sectionText.text = "bodys";
                skinIndex = skinIndex >= bodys.Count ? 0 : skinIndex;
                skinIndex = skinIndex < 0 ? bodys.Count - 1 : skinIndex;
                ShowSkinOnShop(bodys[skinIndex]);
                break;
            case StoreSection.Hands:
                sectionText.text = "hands";
                skinIndex = skinIndex >= hands.Count ? 0 : skinIndex;
                skinIndex = skinIndex < 0 ? hands.Count - 1 : skinIndex;
                ShowSkinOnShop(hands[skinIndex]);
                break;
            case StoreSection.Feets:
                sectionText.text = "feets";
                skinIndex = skinIndex >= feets.Count ? 0 : skinIndex;
                skinIndex = skinIndex < 0 ? feets.Count - 1 : skinIndex;
                ShowSkinOnShop(feets[skinIndex]);
                break;
        }
    }

    /// <summary>
    /// Shows the selected skin on the shop UI
    /// </summary>
    private void ShowSkinOnShop(Skin skinToShow)
    {
        selectedSkin = skinToShow;
        if (characterRepresentation.CompareSkin(selectedSkin, settedSection))
        {
            buyButton.interactable = false;
        }
        else buyButton.interactable = true;
        skinNameText.text = selectedSkin.skinName;
        priceText.text = $"${selectedSkin.price}";
        buyButtonText.text = selectedSkin.bought ? "Equip" : "Buy";

        skinShowcaseRenderer.sprite = skinToShow.icon;
    }

    /// <summary>
    /// Buys and equip skin
    /// </summary>
    public void BuySkin()
    {
        MoneyController moneyController = FindObjectOfType<MoneyController>();
        if (selectedSkin.price <= moneyController.moneyAcount || selectedSkin.bought)
        {
            if(!selectedSkin.bought)moneyController.AddMoney(-selectedSkin.price);
            selectedSkin.bought = true;
            PlayerBehaviour[] playerBehaviours = FindObjectsOfType<PlayerBehaviour>();
            foreach(PlayerBehaviour pb in playerBehaviours) pb.SetSkin(selectedSkin, settedSection, settedDirection);
        }
        ShowSkinOnShop(selectedSkin);
    }

    /// <summary>
    /// Sells and unequip skin
    /// </summary>
    public void SellSkin()
    {
        MoneyController moneyController = FindObjectOfType<MoneyController>();
        if (selectedSkin.bought)
        {
            moneyController.AddMoney(selectedSkin.price);
            selectedSkin.bought = false;
            if (characterRepresentation.CompareSkin(selectedSkin, settedSection))
            {
                PlayerBehaviour[] playerBehaviours = FindObjectsOfType<PlayerBehaviour>();
                foreach (PlayerBehaviour pb in playerBehaviours) pb.SetSkin(settedSection, settedDirection);
            }
        }
        ShowSkinOnShop(selectedSkin);
    }

    public void SetSection(int newSection)
    {
        settedSection = (StoreSection)newSection;
        ShowSkinSection();
    }
}
