using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Punch : MonoBehaviour
{ 
    
    private ControllerManager controllerManager;
    private new Rigidbody rigidbody;

    public bool punchCollidingWithObject;

    public float punchCollisionTimer;
    public float punchCollisionTimerStart;
    public float punchCollisionTimerEnd;
    
    //If either are true then you cannot punch the object
    [Header("Punched & Hit Ground")]
    [Tooltip("If set to true then the object cannot be punched. It is automatically set to true after being punched")]
    public bool punched;
    [Tooltip("If set to true then the object cannot be punched. It is automatically set to true after hitting the ground")]
    public bool hitGround;
    
    private String punchingFistUsed;

    //Change punchVelMultiplier to modify how much force the object gets when punched (Higher makes it go farther/faster)
    [Header("Punch Force")] 
    private float punchForceMultiplier;
    
    [Tooltip("A higher value will apply more force to the object after it is punched in addition to the force the speed of the punch itself applies.")]
    public float punchVelMultiplier = 40;
    
    [Header("Punch Threshold")] [Range(0f, 5f)]
    [Tooltip("The punch velocity need to exceed this value for the punch to count. This value can go from 0 to 5 inclusive.")]
    public float punchVelThreshold;

    [Header("Punch Results")] 
    [Tooltip("Automatically set to true if the last punch qualified as a successful punch")]
    public bool lastPunchWasGood;

    //Add some variables here if you need to test things
    [Header("Testing and Debug")]
    public Transform testbox;
    public bool showDebugLines;
    
    //Gather necessary components
    private void Start()
    {
        controllerManager = GameObject.FindGameObjectWithTag("LeftFist").GetComponentInParent<ControllerManager>();
        rigidbody = GetComponent<Rigidbody>();
        
        //No gravity at start (testing)
       // rigidbody.useGravity = false;
    }
    
    /// <summary>
    /// Run whenever the fish becomes enabled
    /// </summary>
    private void OnEnable()
    {
        punched = false;
        hitGround = false;
        showDebugLines = true;
    }

    /// <summary>
    /// I think you can safely ignore this method for now
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        Reset();
    }
    
    
    /// <summary>
    /// When the fist collides with the object it checks which fist it was that collided
    /// it then calculates where the punch was on the object
    /// It then calls the PunchObject method specifying the fist used for the punch and the direction to send the object
    /// Checks if the object hits the ground. If true then sets the hitGround var to true
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        // reset testbox
        Reset();
        
        //check the impact angle/location
        Vector3 avgPoint = Vector3.zero;
        foreach (ContactPoint p in other.contacts)
            avgPoint += p.point;
        
        avgPoint /= other.contacts.Length; 

        Vector3 dir = (avgPoint - transform.position).normalized;
        
        //make unpunchable if object has hit the ground
        if (other.transform.CompareTag("Ground")) {hitGround = true;}

        //Calls punchObject method with fist and direction as parameters
        if (other.gameObject.CompareTag("LeftFist"))
        {
            if (controllerManager._leftGrip != 1f)
            { Debug.Log("Punch does not qualify as the player did not make a fist"); return; }
            
            Debug.Log("Registered Left Fist Hit");
            PunchObject("LeftFist", dir);
        }
        else if (other.gameObject.CompareTag("RightFist"))
        {
            if (controllerManager._rightGrip != 1f)
            { Debug.Log("Punch does not qualify as the player did not make a fist"); return; }
            
            Debug.Log("Registered Right Fist Hit");
            PunchObject("RightFist", dir);
        }
        
        return;
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("LeftFist"))
        {
            punchCollisionTimerEnd = Time.time; 
            Debug.Log("CollisionL Time = " + (punchCollisionTimerEnd - punchCollisionTimerStart));
        }

        else if (other.gameObject.CompareTag("RightFist"))
        {
            punchCollisionTimerEnd = Time.time;
            Debug.Log("CollisionR Time = " + (punchCollisionTimerEnd - punchCollisionTimerStart));
        }
    }


    /// <summary>
    /// Checks which fist was used for the punch and then applies a force to the object
    /// The force corresponds to the var direction that is passed in from the OnCollisionEnter
    /// and by the velocity of the punch                               
    /// </summary>
    private void PunchObject(String fistUsed, Vector3 direction)
    {
        rigidbody.useGravity = true;
        
        // Do not allow punches if the object has already been punched/has hit the ground
        if (punched || hitGround) { Debug.Log("Punch does not qualify as it has already been punched or hit the ground"); return; }
        
        punchCollisionTimerStart = Time.time;
        punched = true;
        // when punched = true, punch has just now collided with fish, so punchCollidingWithObject is also true now.
        //Set all the other parts of the fish to be unpunchable
        foreach (Punch punchScript in GetComponentsInParent<Punch>())
        {
            if (punchScript != null)
                punchScript.punched = true;
        }
        foreach (Punch punchScript in GetComponentsInChildren<Punch>())
        {
            if (punchScript != null)
                punchScript.punched = true;
        }

        var cubeLaunchDir = Vector3.zero;
        
        //Apply force to the object depending on the velocity, the specified multiplier, and the direction
        if (fistUsed.Equals("LeftFist"))
        {
            //Do not register punch if punch force was too weak
            if (controllerManager.leftVelMagnitude < punchVelThreshold)
            { Debug.Log("Punch Velocity was too weak"); return; }
            
            punchForceMultiplier = controllerManager.leftVelMagnitude * punchVelMultiplier;
            cubeLaunchDir = direction * -punchForceMultiplier;
            Debug.Log("Punched with Left Fist with Force of " + punchForceMultiplier + "\nand a Direction of " + cubeLaunchDir);
        }

        if (fistUsed.Equals("RightFist"))
        {
            //Do not register punch if punch force was too weak
            if (controllerManager.rightVelMagnitude < punchVelThreshold) 
            { Debug.Log("Punch Velocity was too weak"); return; }
            
            punchForceMultiplier = controllerManager.rightVelMagnitude * punchVelMultiplier;
            cubeLaunchDir = direction * -punchForceMultiplier;
            Debug.Log("Punched with Right Fist with Force of " + (punchForceMultiplier) + "\nand a Direction of " + cubeLaunchDir);
        }
        
        rigidbody.AddForce(cubeLaunchDir, ForceMode.VelocityChange);
        
        //Add a slight upwards force
        //rigidbody.AddForce(transform.up * (punchForceMultiplier / 3), ForceMode.VelocityChange);
        if (showDebugLines)
            Debug.DrawLine(transform.position, transform.position + cubeLaunchDir, Color.red, 2.5f);
        
        lastPunchWasGood = true;
    }

    private void Reset()
    {
        if (name.Equals("Reset"))
        { 
            Debug.Log("Resetting Testbox Position"); 
            testbox.GetComponent<Rigidbody>().velocity = Vector3.zero;
            testbox.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            testbox.GetComponent<Rigidbody>().useGravity = false;
            testbox.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
            punched = false;
            testbox.GetComponent<Punch>().punched = false;
            testbox.GetComponent<Punch>().hitGround = false;
            testbox.GetComponent<Punch>().lastPunchWasGood = false;
            testbox.GetComponent<BoxCollider>().enabled = true;
            testbox.position = new Vector3(0.67f, 1.229f, 0f); 
            return;
        }
    }
}
