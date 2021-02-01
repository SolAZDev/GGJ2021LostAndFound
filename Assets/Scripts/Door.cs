using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public SpriteRenderer minimapIcon;
    public Items item;
    public PuzzleSwitch puzzleSwitch;
    public SpriteRenderer keyIcon;

    public void SetMinimapIconColor(Color c)
    {
        minimapIcon.color = c;
    }

    public void SetKeyItem(Items keyItem)
    {
        item = keyItem;
        puzzleSwitch.RequiredItem = keyItem;
    }

    public void SetKeyIcon(Sprite icon)
    {
        keyIcon.sprite = icon;
    }
}
