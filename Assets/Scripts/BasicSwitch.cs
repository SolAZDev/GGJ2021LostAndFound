using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwitch : MonoBehaviour
{
    public bool OnEnterAndExit = false, Toggable = true;
    public UltEvents.UltEvent OnEnter, OnToggle, OnExit;
    public void Toggle() { OnToggle.Invoke(); }
    private void OnTriggerEnter(Collider other) { print("ReToggled"); if (OnEnterAndExit) { OnExit.Invoke(); } }
    private void OnTriggerExit(Collider other) { print("DeToggled"); if (OnEnterAndExit) { OnExit.Invoke(); } }

    public void SendMessageToGameManager(string msg, bool boolParam) { GameManager.instance.SlowPlayerDown(boolParam); }

}
