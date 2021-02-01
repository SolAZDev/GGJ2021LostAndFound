using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Items
{
    HEALTH,
    KEY1, KEY2, KEY3, KEY4, KEY5, KEY6,
    TOTEM1, TOTEM2,
    GOAL
}
public class BasicItem : MonoBehaviour
{
    public Sprite Icon;
    public Items Kind;

    public ParticleSystem particleSystem;

    bool isPicked = false;

    public virtual void Start()
    {
        TargetingStop();
    }

    public virtual void TargetingStart()
    {
        if (isPicked) return;

        if(particleSystem != null)
        {
            particleSystem?.Play();
        }
    }

    public virtual void TargetingStop()
    {
        if (isPicked) return;

        if(particleSystem != null)
        {
            particleSystem.Stop();
            particleSystem.Clear();
        }
    }

    public virtual void AddedToInventory()
    {
        TargetingStop();
        isPicked = true;
        gameObject.SetActive(false);
    }

    public virtual void PlacedAtPedestal()
    {
        //gameObject.SetActive(true);
        //particleSystem?.Stop();
        //particleSystem?.Clear();
        //GetComponent<MeshRenderer>().enabled = true;
        //GetComponent<Collider>().enabled = true;
    }
}
