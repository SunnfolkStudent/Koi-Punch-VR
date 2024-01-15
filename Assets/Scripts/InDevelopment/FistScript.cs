using System;
using UnityEngine;

public class FistScript : MonoBehaviour
{

    private ControllerManager controllerManager;

    private String whichFistUsed;
    
    // Update is called once per frame
    private void Start()
    {
        controllerManager = GetComponentInParent<ControllerManager>();
        whichFistUsed = gameObject.tag;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.TryGetComponent(out IPunchable punchableObject))
        {
                punchableObject.PunchObject(controllerManager, whichFistUsed);
        }
    }
}
