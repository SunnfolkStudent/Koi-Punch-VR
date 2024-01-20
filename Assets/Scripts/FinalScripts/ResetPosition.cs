using System;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] private InputActionReference leftResetButton;
    [SerializeField] private InputActionReference rightResetButton;

    //public Keyboard Keyboard;
    //private bool _resetPosKeyboard;
    
    private bool _resetPosLeft;
    private bool _resetPosRight;
    
    public Transform head; 
    public Transform origin;
    public Transform target;
    
    [ContextMenu("Test Starting Pos")]
    public void ResetPos()
    {
        Vector3 offset = head.position - origin.position;
        offset.y = 0;
        origin.position = target.position - offset;

        Vector3 targetForward = target.forward;
        targetForward.y = 0;
        Vector3 cameraForward = head.forward;
        cameraForward.y = 0;

        float angle = Vector3.SignedAngle(cameraForward, targetForward, Vector3.up);
        
        origin.RotateAround(head.position, Vector3.up, angle);
    }
    void Start()
    {
       ResetPos();
    }

    private void Update()
    {
        _resetPosLeft = leftResetButton.action.WasPressedThisFrame();
        _resetPosRight = rightResetButton.action.WasPressedThisFrame();
        //_resetPosKeyboard = Keyboard.aKey.wasPressedThisFrame;
        
        if (_resetPosLeft || _resetPosRight)
        {
            ResetPos();
        }
    }
}
