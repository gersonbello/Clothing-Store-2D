using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A way to create custom skins for each side the character looks
/// </summary>
[CreateAssetMenu(fileName = "Skin")]
public class Skin : ScriptableObject
{
    [Header("Skin Status")]
    public bool bought;

    [Header("Skin Configuration")]
    public string skinName;
    public int price;
    public Sprite icon;
    public Sprite Up;
    public Sprite Down;
    public Sprite Right;
    public Sprite Left;
}
