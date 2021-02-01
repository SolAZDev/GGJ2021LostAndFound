using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSwitch : MonoBehaviour
{
    public bool OnEnterAndExit = false, Toggable = true;
    public UltEvents.UltEvent OnEnter, OnToggle, OnExit;
    public void Toggle() { OnToggle.Invoke(); }
    private void OnTriggerEnter(Collider other) { if (OnEnterAndExit) { OnExit.Invoke(); } }
    private void OnTriggerExit(Collider other) { if (OnEnterAndExit) { OnExit.Invoke(); } }

}
