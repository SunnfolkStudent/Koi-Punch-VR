using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticManager : MonoBehaviour
{
    [Header("Controller")]
    public XRBaseController leftController, rightController;

    [Header("Test Bools")] 
    public bool leftFistTest;
    public bool rightFistTest;
    
    [Header("Fish Punch")]
    static bool leftFishPunch;
    static bool rightFishPunch;
    
    [Header("Wood Punch")]
    static bool leftWoodPunch; 
    static bool rightWoodPunch;
    
    [Header("Zen Punch 1")]
    static bool leftZenPunch1;
    static bool rightZenPunch1;
    
    [Header("Zen Punch 2")]
    static bool leftZenPunch2;
    static bool rightZenPunch2;
    
    [Header("Zen Punch 3")]
    static bool leftZenPunch3;
    static bool rightZenPunch3;
    
    [Header("Zen Charge")]
    static bool leftZenCharge;
    static bool rightZenCharge;

    [Range(0,0.8f)]
    static float zenCharge = 0 ;
    static float zenAmplifier = 0.1f;
    

    private void Update()
    {
        leftFistTest = leftFishPunch;
        rightFistTest = rightFishPunch;
        
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
        if (leftZenCharge) {zenCharge += zenAmplifier * Time.deltaTime; LeftZenCharge();}
        if (rightZenCharge) { zenCharge += zenAmplifier * Time.deltaTime; RightZenCharge();}
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
    {
        leftController.SendHapticImpulse(1, 0.1f);}
    
    [ContextMenu("Test Right ZenPunch 3")]
    private void RightZenPunch3()
    {rightController.SendHapticImpulse(1, 0.1f);}
    
    [ContextMenu("Test Left Zen Charge")]
    private void LeftZenCharge() 
    {leftController.SendHapticImpulse(zenCharge, 0.1f);}
    
    [ContextMenu("Test Right Zen Charge")]
    private void RightZenCharge() 
    {rightController.SendHapticImpulse(zenCharge, 0.1f);}
}
