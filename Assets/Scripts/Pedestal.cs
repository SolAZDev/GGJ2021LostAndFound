using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : BasicSwitch
{
    public Transform itemCenter;

    BasicItem placedItem;

    public void PlaceItem(BasicItem item)
    {
        placedItem = item;
        item.transform.position = itemCenter.position;
        item.PlacedAtPedestal();
    }
}
