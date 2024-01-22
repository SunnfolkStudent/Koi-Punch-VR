using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticManager : MonoBehaviour
{
    [Header("Controller")]
    public XRBaseController leftController;
    public XRBaseController rightController;
    
    [Header("Right Button Inputs")]
    [SerializeField] private InputActionReference aButton;
    [SerializeField] private InputActionReference bButton;
    
    [Header("Left Button Inputs")]
    [SerializeField] private InputActionReference xButton;
    [SerializeField] private InputActionReference yButton;

    [Header("Button input bools")] 
    private bool _Abutton;
    private bool _Bbutton;
    private bool _Xbutton;
    private bool _Ybutton;
    
    [Header("Button Input Bools")]
    public static bool zenChargeing;

    [Header("Test Bools")] 
    public bool leftFistTest;
    public bool rightFistTest;
    
    [Header("Fish Punch")]
    public static bool leftFishPunch;

    public static bool rightFishPunch;
    
    [Header("Wood Punch")]
    public static bool leftWoodPunch; 
    public static bool rightWoodPunch;
    
    [Header("Zen Punch 1")]
    public static bool leftZenPunch1;
    public static bool rightZenPunch1;
    
    [Header("Zen Punch 2")]
    public static bool leftZenPunch2;
    public static bool rightZenPunch2;
    
    [Header("Zen Punch 3")]
    public static bool leftZenPunch3;
    public static bool rightZenPunch3;
    
    [Header("Zen Charge")]
    public static bool zenCharge;

    [Header("HapticCancel")] 
    public static bool cancelHaptics;

    [Header("zenChargeIntensity")]
    [Range(0,0.8f)]
    [SerializeField] private float zenChargeIntensity = 0;
    [SerializeField] private float zenAmplifier;
    
    

    private void Update()
    {
        _Abutton = aButton.action.IsPressed();
        _Bbutton = bButton.action.IsPressed();
        _Xbutton = xButton.action.IsPressed();
        _Ybutton = yButton.action.IsPressed();

        if (_Abutton || _Bbutton || _Xbutton || _Ybutton)
        {
            zenChargeing = true;
        }
        else
        {
            zenChargeing = false;
        }
         
        if (zenChargeing)
         {
             Debug.Log("ZEN IS CHARGING");   
         }
        zenAmplifier = 0.8f / SpecialAttackScript.timeToCharge;
        
        if (leftFishPunch) {LeftFishPunch();} 
        if (rightFishPunch) {RightFishPunch();}
        if (leftWoodPunch) {LeftWoodPunch();} 
        if (rightWoodPunch) {RightWoodPunch();}
        if (leftZenPunch1) {LeftZenPunch1();} 
        if (rightZenPunch1) {RightZenPunch1();}
        if (leftZenPunch2) {LeftZenPunch2();} 
        if (rightZenPunch2) {RightZenPunch2();}
        if (leftZenPunch3) {LeftZenPunch3();} 
        if (rightZenPunch3) {RightZenPunch3();}
        if (zenCharge) 
        {
            zenChargeIntensity += zenAmplifier * Time.deltaTime; 
            ZenCharge();
        }
        else
        {
            zenChargeIntensity = 0;
        }

        if (cancelHaptics)
        {
            StopRumble();
        }

        leftWoodPunch = false;
        rightWoodPunch = false;
        leftZenPunch1 = false;
        rightZenPunch1 = false;
        leftZenPunch2 = false;
        rightZenPunch2 = false;
        leftZenPunch3 = false;
        rightZenPunch3 = false;
    }
    
    [ContextMenu("Test Left Fish Punch")]
    private void LeftFishPunch()
    {leftController.SendHapticImpulse(0.2f, 0.1f);} 
    
    [ContextMenu("Test Right Fish Punch")]
    private void RightFishPunch()
    {rightController.SendHapticImpulse(0.2f , 0.1f);}
    
   [ContextMenu("Test Left Wood Punch")]
    private void LeftWoodPunch()
    {leftController.SendHapticImpulse(0.35f, 0.1f);}
    
    [ContextMenu("Test Right Wood Punch")]
    private void RightWoodPunch()
    {rightController.SendHapticImpulse(0.35f, 0.1f);}
    
    [ContextMenu("Test Left Zen Punch 1")]
    private void LeftZenPunch1()
    {leftController.SendHapticImpulse(0.5f, 0.1f);}
    [ContextMenu("Test Right Zen Punch 1")]
    private void RightZenPunch1()
    {rightController.SendHapticImpulse(0.5f, 0.1f);}
    
    [ContextMenu("Test Left Zen Punch 2")]
    private void LeftZenPunch2()
    {leftController.SendHapticImpulse(0.7f, 0.1f);}
    
    [ContextMenu("Test Right Zen Punch 2")]
    private void RightZenPunch2()
    {rightController.SendHapticImpulse(0.7f, 0.1f);}

    [ContextMenu("Test Left ZenPunch 3")]
    private void LeftZenPunch3()
    {leftController.SendHapticImpulse(1, 0.1f);}
    
    [ContextMenu("Test Right ZenPunch 3")]
    private void RightZenPunch3()
    {rightController.SendHapticImpulse(1, 0.1f);}

    [ContextMenu("Test Left Zen Charge")]
    private void ZenCharge()
    {
        leftController.SendHapticImpulse(zenChargeIntensity, 2);
        rightController.SendHapticImpulse(zenChargeIntensity, 2);
    }
    private void StopRumble() 
    {
        leftController.SendHapticImpulse(0, 0);
        rightController.SendHapticImpulse(0, 0);
    }
}
