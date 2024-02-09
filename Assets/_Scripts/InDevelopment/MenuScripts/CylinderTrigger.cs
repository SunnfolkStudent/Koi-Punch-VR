using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderTrigger : MonoBehaviour
{
    public static bool LeftHandCanHit = true;
    public static bool RightHandCanHit = true;

    private void Start()
    {
        LeftHandCanHit = true;
        RightHandCanHit = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LeftFist"))
        {
            LeftHandCanHit = true;
        }
        else if (other.gameObject.CompareTag("RightFist"))
        {
            RightHandCanHit = true;
        }
    }
}
