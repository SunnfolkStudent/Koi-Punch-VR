using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchManager : MonoBehaviour
{ 
    
    private ControllerManager controllerManager;

    private new Rigidbody rigidbody;

    public float punchForceMultiplier = 1f;
    
    private void Start()
    {
        controllerManager = GameObject.FindGameObjectWithTag("LeftFist").GetComponentInParent<ControllerManager>();
        
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftFist"))
        {
            Debug.Log("Registered Left Fist Hit");
            Punch("LeftFist");
            
        }
        else if (other.CompareTag("RightFist"))
        {
            Debug.Log("Registered Right Fist Hit");
            Punch("RightFist");
        }
    }

    private void Punch(String fistUsed)
    {
        if (fistUsed.Equals("LeftFist"))
        {
            Debug.Log("left shit");
            rigidbody.velocity = controllerManager.leftControllerVelocity;
        }

        if (fistUsed.Equals("RightFist"))
        {
            Debug.Log("right shit");
            rigidbody.velocity = controllerManager.rightControllerVelocity;
        }
    }
}
