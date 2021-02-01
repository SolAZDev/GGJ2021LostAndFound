using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform KeyItemPlacement, PlayerPos;
    public Door door;
    public SpriteRenderer doorSymbol;

    private void Awake()
    {
        //PlayerPos = transform;
    }
}
