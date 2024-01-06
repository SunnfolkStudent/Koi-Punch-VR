using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

using TMPro;

public class ControllerManager : MonoBehaviour
{
    //Declare Variables

    [Header("Velocity")] 
    public Vector3 leftControllerVelocity;
    public Vector3 rightControllerVelocity;

    //Euler angles and quaternions because fuck quaternions
    [Header("Rotation in Euler Angles")] 
    public Vector3 leftControllerAngle;
    public Vector3 rightControllerAngle;
    [Header("Rotation in Quaternions")] 
    public Quaternion leftControllerQuaternionAngle;
    public Quaternion rightControllerQuaternionAngle;

    [Header("Testing (Please Ignore)")]
    [SerializeField] private GameObject objectToTestOn;
    [SerializeField] private bool needsTesting;
    
    void Start() { }

    void Update()
    {
        if (needsTesting)
            Test();
    }

    void FixedUpdate()
    {
        UpdateInput();
    }

    // A function to test things inside of
    // Do whatever you want in here lmfao
    private void Test()
    {
        objectToTestOn.GetComponent<TextMeshPro>().text =
            "Left Velocity: " + leftControllerVelocity + "\nRight Velocity: " + rightControllerVelocity
            + "\nLeft Angle: " + leftControllerAngle + "\nRight Angle: " + rightControllerAngle
            + "\nLeft Quaternion: " + leftControllerQuaternionAngle + "\nRight Quaternion: " + rightControllerQuaternionAngle;
    }
    
    // Update the velocity variables with the current controller velocity
    // Update angle variables with current controller angle in both euler angles and quaternions
    private void UpdateInput()
    {
        List<XRNodeState> nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);
        foreach (XRNodeState node in nodes)
        {
            if (node.nodeType == XRNode.LeftHand)
            {
                node.TryGetVelocity(out leftControllerVelocity);
                node.TryGetRotation(out leftControllerQuaternionAngle);
                
                leftControllerAngle = leftControllerQuaternionAngle.eulerAngles;
            }

            if (node.nodeType == XRNode.RightHand)
            {
                node.TryGetVelocity(out rightControllerVelocity);
                node.TryGetRotation(out rightControllerQuaternionAngle);

                rightControllerAngle = rightControllerQuaternionAngle.eulerAngles;
            }
        }
    }
}
