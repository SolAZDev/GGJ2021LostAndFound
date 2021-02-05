using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer minimapIcon;
    public Items item;
    public PuzzleSwitch puzzleSwitch;
    public SpriteRenderer keyIcon;
    public Light glow;

    public void SetMinimapIconColor(Color c)
    {
        minimapIcon.color = c;
    }

    public void SetKeyItem(Items keyItem, Sprite icon, Color color)
    {
        item = keyItem;
        puzzleSwitch.RequiredItem = keyItem;
        keyIcon.sprite = icon;
        keyIcon.color = color;
        glow.color = color;
    }

}
