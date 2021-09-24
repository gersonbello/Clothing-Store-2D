using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A way to create custom skins for each side the character looks
/// </summary>
[CreateAssetMenu(fileName = "Skin")]
public class Skin : ScriptableObject
{
    [Header("Hat")]
    public Sprite hatUp;
    public Sprite hatDown;
    public Sprite hatRight;
    public Sprite hatLeft;

    [Header("Head")]
    public Sprite headUp;
    public Sprite headDown;
    public Sprite headRight;
    public Sprite headLeft;

    [Header("Body")]
    public Sprite bodyUp;
    public Sprite bodyDown;
    public Sprite bodyRight;
    public Sprite bodyLeft;

    [Header("Hand")]
    public Sprite handsUp;
    public Sprite handsDown;
    public Sprite handsRight;
    public Sprite handsLeft;

    [Header("Feet")]
    public Sprite feetUp;
    public Sprite feetDown;
    public Sprite feetRight;
    public Sprite feetLeft;
}
