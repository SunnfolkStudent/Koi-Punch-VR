using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerRumble : MonoBehaviour
{
    public static ControllerRumble Instance;
    
    private List<InputDevice> _leftDevices = new List<InputDevice>();
    private List<InputDevice> _rightDevices = new List<InputDevice>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, _leftDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, _rightDevices);
    }

    // Update is called once per frame
    public void LeftControllerRumbling(float rumbleStrength, float rumbleDuration)
    {
        foreach (InputDevice device in _leftDevices)
        {
            device.SendHapticImpulse(0, rumbleStrength, rumbleDuration);
        }
    }
    
    public void RightControllerRumbling(float rumbleStrength, float rumbleDuration)
    {
        foreach (InputDevice device in _rightDevices)
        {
            device.SendHapticImpulse(0, rumbleStrength, rumbleDuration);
        }
    }
    
    public void StopRightRumble()
    {
        foreach (InputDevice device in _rightDevices)
        {
            device.StopHaptics();
        }
    }
    
    public void StopLeftRumble()
    {
        foreach (InputDevice device in _leftDevices)
        {
            device.StopHaptics();
        }
    }
}
