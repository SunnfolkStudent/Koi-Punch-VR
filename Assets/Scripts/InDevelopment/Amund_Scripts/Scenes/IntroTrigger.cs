using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class IntroTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        gameObject.Destroy();
    }
}
