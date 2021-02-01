using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSwitch : BasicSwitch
{
    public Items RequiredItem;
    public Transform ItemPos;
    public UltEvents.UltEvent OnWrongItem;

    public bool CheckCorrectItem(BasicItem item)
    {
        if (item?.Kind != RequiredItem)
        {
            OnWrongItem.Invoke();
            return false;
        }
        else
        {
            OnToggle.Invoke();
            return true;
        }
    }

    public void PlaceItem(BasicItem item)
    {
        //item.transform.position = ItemPos.position;
        //item.transform.rotation = ItemPos.rotation;
        //item.transform.localScale = ItemPos.localScale;
        //item.gameObject.SetActive(true);
        item.PlacedAtPedestal();
        Destroy(item);
        Destroy(transform.parent.gameObject);
    }
}
