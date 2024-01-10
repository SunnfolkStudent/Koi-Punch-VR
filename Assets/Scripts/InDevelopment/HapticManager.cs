using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticManager : MonoBehaviour
{
    public XRBaseController leftController, rightController;
    
    [SerializeField] private InputActionReference leftTopButtonAction;
    [SerializeField] private InputActionReference rightTopButtonAction;

    [SerializeField] private float _leftTopButton;
    [SerializeField] private float _rightTopButton;

    [Range(0,0.8f)]
    public float zenCharge = 0 ;
    public float zenAmplifier = 0.1f;
    

    private void Update()
    {
        _leftTopButton = leftTopButtonAction.action.ReadValue<float>();
        _rightTopButton = rightTopButtonAction.action.ReadValue<float>();

        if (_rightTopButton != 0 || _leftTopButton != 0)
        {
            zenCharge += zenAmplifier * Time.deltaTime;
            ZenCharge();
        }
        else if (zenCharge != 0)
        {
            zenCharge = 0;
        }
        
    }

    [ContextMenu("Test Fish Punch")]
    private void FishPunch()
    {
        leftController.SendHapticImpulse(0.2f, 0.1f);
        rightController.SendHapticImpulse(0.2f , 0.1f);
    }
    
   [ContextMenu("Test Wood Punch")]
    private void WoodPunch()
    {
        leftController.SendHapticImpulse(0.35f, 0.1f);
        rightController.SendHapticImpulse(0.35f, 0.1f);
    }
    
    [ContextMenu("Test Zen Punch 1")]
    private void ZenPunch1()
    {
        leftController.SendHapticImpulse(0.5f, 0.1f);
        rightController.SendHapticImpulse(0.5f, 0.1f);
    }
    
    [ContextMenu("Test Zen Punch 2")]
    private void ZenPunch2()
    {
        leftController.SendHapticImpulse(0.7f, 0.1f);
        rightController.SendHapticImpulse(0.7f, 0.1f);
    }
    
    [ContextMenu("Test Zen Charge")]
    private void ZenCharge()
    {
        if (_leftTopButton != 0 )
        {leftController.SendHapticImpulse(zenCharge, 0.1f);}
       
        if (_rightTopButton != 0 )
        {rightController.SendHapticImpulse(zenCharge, 0.1f);}
    }
    
    [ContextMenu("Test ZenPunch 3")]
    private void ZenPunch3()
    {
        rightController.SendHapticImpulse(1, 0.1f);
        leftController.SendHapticImpulse(1, 0.1f);
    }
}
