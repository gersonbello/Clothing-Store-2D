using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    /// <summary>
    /// Rotate to next direction
    /// </summary>
    public void NextDirection()
    {
        settedDirection = (Direction)Mathf.Repeat(settedDirection.GetHashCode() + 1, 4);
        characterRepresentation.ChangeSkin(settedDirection);
    }
    /// <summary>
    /// Rotate to previous direction
    /// </summary>
    public void PreviousDirection()
    {
        settedDirection = (Direction)Mathf.Repeat(settedDirection.GetHashCode() - 1, 4);
        characterRepresentation.ChangeSkin(settedDirection);
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
                skinIndex = skinIndex >= hats.Count ? 0 : skinIndex;
                skinIndex = skinIndex < 0 ? hats.Count - 1 : skinIndex;
                ShowSkinOnShop(hats[skinIndex]);
                break;
        }
    }

    /// <summary>
    /// Shows the selected skin on the shop UI
    /// </summary>
    private void ShowSkinOnShop(Skin skinToShow)
    {
        selectedSkin = skinToShow;
        skinShowcaseRenderer.sprite = skinToShow.Right;

        //switch (settedDirection)
        //{
        //    case Direction.up: skinShowcaseRenderer.sprite = skinToShow.Up; break;
        //    case Direction.down: skinShowcaseRenderer.sprite = skinToShow.Down; break;
        //    case Direction.left: skinShowcaseRenderer.sprite = skinToShow.Left; break;
        //    case Direction.right: skinShowcaseRenderer.sprite = skinToShow.Right; break;
        //}
    }

    /// <summary>
    /// Buys and equip skin
    /// </summary>
    public void BuySkin()
    {
        characterRepresentation.SetSkin(selectedSkin, settedSection);
    }
}
