using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using UrFairy;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    public float Speed = 5, RunSpeedMult = 2f;
    public float rotSpeed = 5, mouseSensitivity = .5f, runTimeLimit = 15, recoveryTime = 30;
    public Animator cameraAnim;
    public Transform cameraRayOffset;
    public Rig NormalFlRig, FocusedRig;

    public float maxRATargetRotation = 20f;
    public Transform RATarget;
    float RATargetBaseRot;

    public UnityEngine.UI.GridLayoutGroup iconGroup;
    public Transform ItemIconHUDPrefab;

    public UnityEngine.UI.Image itemImageUI;

    public List<BasicItem> Inventory = new List<BasicItem>();
    Vector2 aimDir = Vector2.zero;
    Vector3 mDir = Vector3.zero;
    Vector3 tempDir = Vector3.zero;
    bool isZoomed = false, isRunning = false, canMove = true, isRecovering = false;
    float runTime = 15f, recTimeOut = 30;

    BasicItem lastItemTargeted = null;

    PlayerInput inputSystem;
    CharacterController cc;
    Animator animator;
    PuzzleSwitch CurSwitch;

    // Start is called before the first frame update
    void Start()
    {
        if (Player.instance == null) { Player.instance = this; }
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        inputSystem = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RATargetBaseRot = RATarget.localRotation.eulerAngles.x;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // Update is called once per frame
    void Update()
    {
        Transform mainCameraTransform = Camera.main.transform;
        tempDir = mainCameraTransform.rotation * new Vector3(mDir.x, Physics.gravity.y / 4, mDir.y);

        animator.SetBool("Walking", mDir.magnitude > 0.1f);
        cc.Move((canMove ? (tempDir * (isRunning ? Speed * RunSpeedMult : Speed)) : Physics.gravity) * Time.deltaTime);

        Ray ray = new Ray(mainCameraTransform.position, cameraRayOffset.localRotation * mainCameraTransform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 1.5f, out hit, 5, LayerMask.GetMask("Item")))
        {
            BasicItem item = hit.collider.GetComponent<BasicItem>();
            if (item != null && item != lastItemTargeted)
            {
                if (lastItemTargeted != null)
                {
                    lastItemTargeted.TargetingStop();
                }

                lastItemTargeted = item;
                item.TargetingStart();
            }
        }
        else
        {
            if (lastItemTargeted != null)
            {
                lastItemTargeted.TargetingStop();
                lastItemTargeted = null;
            }
        }

        //Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + (cameraRayOffset.localRotation * mainCamera.transform.forward * 5), Color.red);
    }

    private void LateUpdate()
    {
        transform.Rotate(0, aimDir.x, 0);

        var curAngle = RATarget.transform.localRotation.eulerAngles;
        curAngle.x += aimDir.y;
        if (curAngle.x >= 180) curAngle.x -= 360;
        curAngle.x = Mathf.Clamp(curAngle.x, RATargetBaseRot - maxRATargetRotation, RATargetBaseRot + maxRATargetRotation);

        var rot = RATarget.transform.localRotation;
        rot.eulerAngles = curAngle;
        RATarget.transform.localRotation = rot;

        runTime -= isRunning ? Time.deltaTime : 0;
        recTimeOut -= (!isRunning && runTime < runTimeLimit && recTimeOut > 0) ? Time.deltaTime : ((recTimeOut < 0) ? -recoveryTime : 0);
        if (recTimeOut >= recoveryTime) { recTimeOut = recoveryTime; }
        if (recTimeOut <= 0) { runTime = runTimeLimit; }
        if (runTime > 0 && isRunning) { animator.SetBool("Running", true); } else { animator.SetBool("Running", false); }
    }

    #region Controls
    public void OnMove(InputValue input) { mDir = isZoomed ? Vector2.zero : input.Get<Vector2>(); }

    public void OnMouseLook(InputValue val) { aimDir = val.Get<Vector2>().normalized * (1 + mouseSensitivity); }
    public void OnJoyLook(InputValue val) { aimDir = val.Get<Vector2>(); }

    public void OnRun() { isRunning = !isRunning; }

    public void OnAim()
    {
        if (InGameHUD.instance.paused) { InGameHUD.instance?.BackToMenu(); }
        isZoomed = !isZoomed;
        NormalFlRig.weight = isZoomed ? 0 : 1;
        FocusedRig.weight = isZoomed ? 1 : 0;
        cameraAnim?.SetBool("Zoom", isZoomed);
    }
    public void OnUse()
    {
        if (InGameHUD.instance.paused) { InGameHUD.instance?.Resume(); }
        animator.SetTrigger("Use");

        if (lastItemTargeted != null)
        {
            if (lastItemTargeted.Kind == Items.GOAL)
            {
                // We win
                GameManager.instance.LoadScene("WinScene");
            }
            else
            {
                Inventory.Add(lastItemTargeted);
                lastItemTargeted.AddedToInventory();
                itemImageUI.sprite = lastItemTargeted.Icon;
                itemImageUI.color = Color.white;

                var obj = Instantiate(ItemIconHUDPrefab, iconGroup.transform);
                obj.GetComponent<UnityEngine.UI.Image>().sprite = lastItemTargeted.Icon;

                lastItemTargeted = null;
            }
        }

        else if (Inventory.Count > 0 && CurSwitch != null)
        {
            if (CurSwitch.CheckCorrectItem(Inventory[0]))
            {
                CurSwitch.PlaceItem(Inventory[0]);
                Inventory.RemoveAt(0);
            }
            else
            {
                Debug.Log("Placing wrong item");
            }
        }
    }

    public void OnCycleRight() { InGameHUD.instance.CycleItems(1); }
    public void OnCycleLeft() { InGameHUD.instance.CycleItems(-1); }
    public void OnPause() { InGameHUD.instance.Pause(false); }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PuzzleSwitch")
        {
            CurSwitch = other.GetComponent<PuzzleSwitch>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BasicSwitch>() == CurSwitch)
        {
            CurSwitch = null;
        }
    }
}
